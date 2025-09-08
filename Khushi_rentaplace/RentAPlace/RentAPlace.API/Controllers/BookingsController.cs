using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.DTOs;
using RentAPlace.API.Models;
using RentAPlace.API.Services;
using System.Security.Claims;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly RentAPlaceDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(
            RentAPlaceDbContext context, 
            IEmailService emailService,
            ILogger<BookingsController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // Test email endpoint
        [HttpPost("test-email")]
        [AllowAnonymous]
        public async Task<ActionResult> TestEmail()
        {
            try
            {
                var testMessage = new EmailMessage
                {
                    To = "mohammadharis7704@gmail.com", // Your email for testing
                    Subject = "Test Email from RentAPlace",
                    Body = "This is a test email to verify the email system is working.",
                    IsHtml = false
                };

                var result = await _emailService.SendEmailAsync(testMessage);
                
                if (result.Success)
                {
                    return Ok(new { message = "Test email sent successfully!", result });
                }
                else
                {
                    return BadRequest(new { message = "Failed to send test email", error = result.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test email");
                return StatusCode(500, new { message = "Error sending test email", error = ex.Message });
            }
        }

        // Temporary endpoint for testing without authentication
        [HttpPost("simple")]
        [AllowAnonymous]
        public async Task<ActionResult<BookingConfirmationDto>> CreateSimpleBooking(CreateSimpleBookingDto createBookingDto)
        {
            try
            {
                // Use a default user for testing (first user in database)
                var defaultUser = await _context.Users.FirstAsync();
                
                // Validate property exists
                var property = await _context.Properties
                    .Include(p => p.PropertyType)
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(p => p.Id == createBookingDto.PropertyId);

                if (property == null)
                {
                    return NotFound("Property not found");
                }

                // Use default dates if not provided
                var checkInDate = createBookingDto.CheckInDate ?? DateTime.Today.AddDays(1);
                var checkOutDate = createBookingDto.CheckOutDate ?? DateTime.Today.AddDays(3);

                // Validate dates
                if (checkInDate >= checkOutDate)
                {
                    return BadRequest("Check-in date must be before check-out date");
                }

                // Calculate total price
                var nights = (checkOutDate - checkInDate).Days;
                var totalPrice = property.Price * nights;

                // Create booking
                var booking = new Booking
                {
                    PropertyId = createBookingDto.PropertyId,
                    UserId = defaultUser.Id,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    TotalPrice = totalPrice,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Generate confirmation number
                var confirmationNumber = $"BK{booking.Id:D6}";

                // Send email notification to property owner
                _logger.LogInformation("Attempting to send email notification for booking {BookingId} to owner {OwnerEmail}", 
                    booking.Id, property.Owner?.Email);
                await SendBookingConfirmationEmailAsync(booking, property, defaultUser);

                var confirmation = new BookingConfirmationDto
                {
                    BookingId = booking.Id,
                    PropertyName = property.Name,
                    PropertyAddress = $"{property.Address}, {property.City}, {property.State}",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    ConfirmationNumber = confirmationNumber
                };

                return Ok(confirmation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Temporary endpoint for testing without authentication
        [HttpGet("simple")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetSimpleBookings()
        {
            try
            {
                // Use a default user for testing (first user in database)
                var defaultUser = await _context.Users.FirstAsync();
                
                var bookings = await _context.Bookings
                    .Include(b => b.Property)
                        .ThenInclude(p => p.PropertyType)
                    .Include(b => b.Property)
                        .ThenInclude(p => p.Images)
                    .Where(b => b.UserId == defaultUser.Id)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                var bookingDtos = bookings.Select(booking => new BookingResponseDto
                {
                    Id = booking.Id,
                    PropertyId = booking.PropertyId,
                    PropertyName = booking.Property.Name,
                    PropertyAddress = $"{booking.Property.Address}, {booking.Property.City}, {booking.Property.State}",
                    PropertyImage = booking.Property.Images?.FirstOrDefault()?.ImageUrl ?? "/assets/images/placeholder.jpg",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    CreatedAt = booking.CreatedAt,
                    UserId = booking.UserId,
                    UserName = $"{defaultUser.FirstName} {defaultUser.LastName}",
                    UserEmail = defaultUser.Email,
                    PropertyTypeId = booking.Property.PropertyTypeId,
                    PropertyType = booking.Property.PropertyType.Name,
                    Bedrooms = booking.Property.Bedrooms,
                    Bathrooms = booking.Property.Bathrooms,
                    Area = booking.Property.Area
                }).ToList();

                return Ok(bookingDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BookingConfirmationDto>> CreateBooking(CreateBookingDto createBookingDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                // Validate property exists
                var property = await _context.Properties
                    .Include(p => p.PropertyType)
                    .FirstOrDefaultAsync(p => p.Id == createBookingDto.PropertyId);

                if (property == null)
                {
                    return NotFound("Property not found");
                }

                // Validate dates
                if (createBookingDto.CheckInDate >= createBookingDto.CheckOutDate)
                {
                    return BadRequest("Check-in date must be before check-out date");
                }

                // Calculate total price
                var nights = (createBookingDto.CheckOutDate - createBookingDto.CheckInDate).Days;
                var totalPrice = property.Price * nights;

                // Create booking
                var booking = new Booking
                {
                    PropertyId = createBookingDto.PropertyId,
                    UserId = userId,
                    CheckInDate = createBookingDto.CheckInDate,
                    CheckOutDate = createBookingDto.CheckOutDate,
                    TotalPrice = totalPrice,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                // Generate confirmation number
                var confirmationNumber = $"BK{booking.Id:D6}";

                var confirmation = new BookingConfirmationDto
                {
                    BookingId = booking.Id,
                    PropertyName = property.Name,
                    PropertyAddress = $"{property.Address}, {property.City}, {property.State}",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    ConfirmationNumber = confirmationNumber
                };

                return Ok(confirmation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetMyBookings()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                var bookings = await _context.Bookings
                    .Include(b => b.Property)
                        .ThenInclude(p => p.PropertyType)
                    .Include(b => b.Property)
                        .ThenInclude(p => p.Images)
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                var bookingDtos = bookings.Select(booking => new BookingResponseDto
                {
                    Id = booking.Id,
                    PropertyId = booking.PropertyId,
                    PropertyName = booking.Property.Name,
                    PropertyAddress = $"{booking.Property.Address}, {booking.Property.City}, {booking.Property.State}",
                    PropertyImage = booking.Property.Images?.FirstOrDefault()?.ImageUrl ?? "/assets/images/placeholder.jpg",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    CreatedAt = booking.CreatedAt,
                    UserId = booking.UserId,
                    UserName = $"{booking.User.FirstName} {booking.User.LastName}",
                    UserEmail = booking.User.Email,
                    PropertyTypeId = booking.Property.PropertyTypeId,
                    PropertyType = booking.Property.PropertyType.Name,
                    Bedrooms = booking.Property.Bedrooms,
                    Bathrooms = booking.Property.Bathrooms,
                    Area = booking.Property.Area
                }).ToList();

                return Ok(bookingDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("owner")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetOwnerBookings()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                var bookings = await _context.Bookings
                    .Include(b => b.Property)
                        .ThenInclude(p => p.PropertyType)
                    .Include(b => b.Property)
                        .ThenInclude(p => p.Images)
                    .Include(b => b.User)
                    .Where(b => b.Property.OwnerId == userId)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                var bookingDtos = bookings.Select(booking => new BookingResponseDto
                {
                    Id = booking.Id,
                    PropertyId = booking.PropertyId,
                    PropertyName = booking.Property.Name,
                    PropertyAddress = $"{booking.Property.Address}, {booking.Property.City}, {booking.Property.State}",
                    PropertyImage = booking.Property.Images?.FirstOrDefault()?.ImageUrl ?? "/assets/images/placeholder.jpg",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    CreatedAt = booking.CreatedAt,
                    UserId = booking.UserId,
                    UserName = $"{booking.User.FirstName} {booking.User.LastName}",
                    UserEmail = booking.User.Email,
                    PropertyTypeId = booking.Property.PropertyTypeId,
                    PropertyType = booking.Property.PropertyType.Name,
                    Bedrooms = booking.Property.Bedrooms,
                    Bathrooms = booking.Property.Bathrooms,
                    Area = booking.Property.Area
                }).ToList();

                return Ok(bookingDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingResponseDto>> GetBooking(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                var booking = await _context.Bookings
                    .Include(b => b.Property)
                        .ThenInclude(p => p.PropertyType)
                    .Include(b => b.Property)
                        .ThenInclude(p => p.Images)
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.Id == id && (b.UserId == userId || b.Property.OwnerId == userId));

                if (booking == null)
                {
                    return NotFound("Booking not found");
                }

                var bookingDto = new BookingResponseDto
                {
                    Id = booking.Id,
                    PropertyId = booking.PropertyId,
                    PropertyName = booking.Property.Name,
                    PropertyAddress = $"{booking.Property.Address}, {booking.Property.City}, {booking.Property.State}",
                    PropertyImage = booking.Property.Images?.FirstOrDefault()?.ImageUrl ?? "/assets/images/placeholder.jpg",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    Status = booking.Status.ToString(),
                    CreatedAt = booking.CreatedAt,
                    UserId = booking.UserId,
                    UserName = $"{booking.User.FirstName} {booking.User.LastName}",
                    UserEmail = booking.User.Email,
                    PropertyTypeId = booking.Property.PropertyTypeId,
                    PropertyType = booking.Property.PropertyType.Name,
                    Bedrooms = booking.Property.Bedrooms,
                    Bathrooms = booking.Property.Bathrooms,
                    Area = booking.Property.Area
                };

                return Ok(bookingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

                if (booking == null)
                {
                    return NotFound("Booking not found");
                }

                if (booking.Status != BookingStatus.Pending)
                {
                    return BadRequest("Only pending bookings can be cancelled");
                }

                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Booking cancelled successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}/confirm")]
        public async Task<ActionResult> ConfirmBooking(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                var booking = await _context.Bookings
                    .Include(b => b.Property)
                    .FirstOrDefaultAsync(b => b.Id == id && b.Property.OwnerId == userId);

                if (booking == null)
                {
                    return NotFound("Booking not found");
                }

                if (booking.Status != BookingStatus.Pending)
                {
                    return BadRequest("Only pending bookings can be confirmed");
                }

                booking.Status = BookingStatus.Confirmed;
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Booking confirmed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult> CompleteBooking(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not found");
                }

                var booking = await _context.Bookings
                    .Include(b => b.Property)
                    .FirstOrDefaultAsync(b => b.Id == id && b.Property.OwnerId == userId);

                if (booking == null)
                {
                    return NotFound("Booking not found");
                }

                if (booking.Status != BookingStatus.Confirmed)
                {
                    return BadRequest("Only confirmed bookings can be completed");
                }

                booking.Status = BookingStatus.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Booking completed successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private async Task SendBookingConfirmationEmailAsync(Booking booking, Property property, User renter)
        {
            try
            {
                if (property.Owner == null)
                {
                    _logger.LogWarning("Property {PropertyId} has no owner, cannot send email notification", property.Id);
                    return;
                }

                var bookingData = new BookingConfirmationData
                {
                    OwnerEmail = property.Owner.Email,
                    OwnerName = $"{property.Owner.FirstName} {property.Owner.LastName}",
                    RenterName = $"{renter.FirstName} {renter.LastName}",
                    RenterEmail = renter.Email,
                    PropertyName = property.Name,
                    PropertyAddress = $"{property.Address}, {property.City}, {property.State}",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    BookingId = booking.Id,
                    MessageLink = $"{Request.Scheme}://{Request.Host}/messages?booking={booking.Id}"
                };

                var emailResult = await _emailService.SendBookingConfirmationAsync(bookingData);

                if (emailResult.Success)
                {
                    _logger.LogInformation("Booking confirmation email sent successfully to {OwnerEmail} for booking {BookingId}", 
                        property.Owner.Email, booking.Id);
                }
                else
                {
                    _logger.LogError("Failed to send booking confirmation email to {OwnerEmail} for booking {BookingId}: {Error}", 
                        property.Owner.Email, booking.Id, emailResult.ErrorMessage);

                    // Store notification for retry
                    await StoreNotificationForRetryAsync(bookingData, emailResult.ErrorMessage ?? "Unknown error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending booking confirmation email for booking {BookingId}", booking.Id);
                
                // Store notification for retry
                await StoreNotificationForRetryAsync(new BookingConfirmationData
                {
                    OwnerEmail = property.Owner?.Email ?? "unknown@example.com",
                    OwnerName = property.Owner != null ? $"{property.Owner.FirstName} {property.Owner.LastName}" : "Unknown Owner",
                    RenterName = $"{renter.FirstName} {renter.LastName}",
                    RenterEmail = renter.Email,
                    PropertyName = property.Name,
                    PropertyAddress = $"{property.Address}, {property.City}, {property.State}",
                    CheckInDate = booking.CheckInDate,
                    CheckOutDate = booking.CheckOutDate,
                    TotalPrice = booking.TotalPrice,
                    BookingId = booking.Id
                }, ex.Message);
            }
        }

        private async Task StoreNotificationForRetryAsync(BookingConfirmationData data, string errorMessage)
        {
            try
            {
                var notification = new Notification
                {
                    RecipientEmail = data.OwnerEmail,
                    Subject = $"New Booking Confirmation - {data.PropertyName}",
                    Body = GenerateBookingConfirmationHtml(data),
                    Type = "BookingConfirmation",
                    RelatedEntityId = data.BookingId,
                    Status = "Pending",
                    ErrorMessage = errorMessage,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Notification stored for retry: {NotificationId}", notification.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store notification for retry");
            }
        }

        private static string GenerateBookingConfirmationHtml(BookingConfirmationData data)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Booking Confirmation</title>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .booking-details {{ background: white; padding: 20px; border-radius: 8px; margin: 20px 0; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
        .detail-row {{ display: flex; justify-content: space-between; margin: 10px 0; padding: 8px 0; border-bottom: 1px solid #eee; }}
        .detail-label {{ font-weight: bold; color: #555; }}
        .detail-value {{ color: #333; }}
        .total-price {{ font-size: 1.2em; font-weight: bold; color: #667eea; }}
        .cta-button {{ display: inline-block; background: #667eea; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 30px; color: #666; font-size: 0.9em; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>🎉 New Booking Confirmation!</h1>
        <p>You have received a new booking for your property</p>
    </div>
    
    <div class='content'>
        <h2>Booking Details</h2>
        
        <div class='booking-details'>
            <div class='detail-row'>
                <span class='detail-label'>Property:</span>
                <span class='detail-value'>{data.PropertyName}</span>
            </div>
            <div class='detail-row'>
                <span class='detail-label'>Address:</span>
                <span class='detail-value'>{data.PropertyAddress}</span>
            </div>
            <div class='detail-row'>
                <span class='detail-label'>Guest Name:</span>
                <span class='detail-value'>{data.RenterName}</span>
            </div>
            <div class='detail-row'>
                <span class='detail-label'>Guest Email:</span>
                <span class='detail-value'>{data.RenterEmail}</span>
            </div>
            <div class='detail-row'>
                <span class='detail-label'>Check-in Date:</span>
                <span class='detail-value'>{data.CheckInDate:MMMM dd, yyyy}</span>
            </div>
            <div class='detail-row'>
                <span class='detail-label'>Check-out Date:</span>
                <span class='detail-value'>{data.CheckOutDate:MMMM dd, yyyy}</span>
            </div>
            <div class='detail-row'>
                <span class='detail-label'>Total Price:</span>
                <span class='detail-value total-price'>${data.TotalPrice:N2}</span>
            </div>
        </div>

        <p>Please review the booking details and contact the guest if needed.</p>
        
        {(!string.IsNullOrEmpty(data.MessageLink) ? $@"
        <div style='text-align: center;'>
            <a href='{data.MessageLink}' class='cta-button'>View Messages</a>
        </div>
        " : "")}
    </div>
    
    <div class='footer'>
        <p>This is an automated message from RentAPlace.</p>
        <p>Booking ID: #{data.BookingId}</p>
    </div>
</body>
</html>";
        }
    }
}