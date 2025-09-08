using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using RentAPlace.API.Models;
using System.Security.Claims;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly RentAPlaceDbContext _context;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(RentAPlaceDbContext context, ILogger<NotificationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? status = null,
            [FromQuery] string? type = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var query = _context.Notifications
                    .Where(n => n.RecipientEmail == userId); // Assuming email is used as user identifier

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(n => n.Status == status);
                }

                if (!string.IsNullOrEmpty(type))
                {
                    query = query.Where(n => n.Type == type);
                }

                query = query.OrderByDescending(n => n.CreatedAt);

                var totalCount = await query.CountAsync();
                var notifications = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(n => new NotificationDto
                    {
                        Id = n.Id,
                        RecipientEmail = n.RecipientEmail,
                        Subject = n.Subject,
                        Body = n.Body,
                        Type = n.Type,
                        RelatedEntityId = n.RelatedEntityId,
                        Status = n.Status,
                        RetryCount = n.RetryCount,
                        ErrorMessage = n.ErrorMessage,
                        CreatedAt = n.CreatedAt,
                        SentAt = n.SentAt,
                        LastRetryAt = n.LastRetryAt,
                        NextRetryAt = n.NextRetryAt,
                        IsRead = false // You can add this field to the Notification model if needed
                    })
                    .ToListAsync();

                Response.Headers["X-Total-Count"] = totalCount.ToString();
                Response.Headers["X-Page"] = page.ToString();
                Response.Headers["X-Page-Size"] = pageSize.ToString();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notifications");
                return StatusCode(500, "An error occurred while fetching notifications");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<NotificationStatsDto>> GetNotificationStats()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var stats = await _context.Notifications
                    .Where(n => n.RecipientEmail == userId)
                    .GroupBy(n => n.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                var totalNotifications = stats.Sum(s => s.Count);
                var unreadCount = stats.FirstOrDefault(s => s.Status == "Pending")?.Count ?? 0;
                var pendingCount = stats.FirstOrDefault(s => s.Status == "Pending")?.Count ?? 0;
                var failedCount = stats.FirstOrDefault(s => s.Status == "Failed")?.Count ?? 0;

                return Ok(new NotificationStatsDto
                {
                    TotalNotifications = totalNotifications,
                    UnreadCount = unreadCount,
                    PendingCount = pendingCount,
                    FailedCount = failedCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notification stats");
                return StatusCode(500, "An error occurred while fetching notification stats");
            }
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto createDto)
        {
            try
            {
                var notification = new Notification
                {
                    RecipientEmail = createDto.RecipientEmail,
                    Subject = createDto.Subject,
                    Body = createDto.Body,
                    Type = createDto.Type,
                    RelatedEntityId = createDto.RelatedEntityId,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                var notificationDto = new NotificationDto
                {
                    Id = notification.Id,
                    RecipientEmail = notification.RecipientEmail,
                    Subject = notification.Subject,
                    Body = notification.Body,
                    Type = notification.Type,
                    RelatedEntityId = notification.RelatedEntityId,
                    Status = notification.Status,
                    RetryCount = notification.RetryCount,
                    ErrorMessage = notification.ErrorMessage,
                    CreatedAt = notification.CreatedAt,
                    SentAt = notification.SentAt,
                    LastRetryAt = notification.LastRetryAt,
                    NextRetryAt = notification.NextRetryAt,
                    IsRead = false
                };

                return CreatedAtAction(nameof(GetNotifications), new { id = notification.Id }, notificationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification");
                return StatusCode(500, "An error occurred while creating notification");
            }
        }

        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == id && n.RecipientEmail == userId);

                if (notification == null)
                {
                    return NotFound("Notification not found");
                }

                // You can add an IsRead field to the Notification model
                // For now, we'll just return success
                return Ok(new { message = "Notification marked as read" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification as read");
                return StatusCode(500, "An error occurred while marking notification as read");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                var notification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.Id == id && n.RecipientEmail == userId);

                if (notification == null)
                {
                    return NotFound("Notification not found");
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Notification deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification");
                return StatusCode(500, "An error occurred while deleting notification");
            }
        }

        // Admin endpoint to retry failed notifications
        [HttpPost("{id}/retry")]
        [Authorize(Roles = "Admin")] // Add admin role check
        public async Task<IActionResult> RetryNotification(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    return NotFound("Notification not found");
                }

                if (notification.Status != "Failed")
                {
                    return BadRequest("Only failed notifications can be retried");
                }

                // Reset for retry
                notification.Status = "Pending";
                notification.RetryCount = 0;
                notification.ErrorMessage = null;
                notification.NextRetryAt = null;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Notification queued for retry" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrying notification");
                return StatusCode(500, "An error occurred while retrying notification");
            }
        }
    }
}
