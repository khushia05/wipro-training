using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.Services;
using RentAPlace.API.Models;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailTestController> _logger;

        public EmailTestController(IEmailService emailService, ILogger<EmailTestController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send-test")]
        public async Task<ActionResult> SendTestEmail()
        {
            try
            {
                _logger.LogInformation("Starting email test...");

                var message = new EmailMessage
                {
                    To = "mohammadharis7704@gmail.com",
                    Subject = "Test Email from RentAPlace",
                    Body = "This is a test email to verify the email system is working properly.",
                    IsHtml = false
                };

                _logger.LogInformation("Sending email to: {Email}", message.To);
                var result = await _emailService.SendEmailAsync(message);

                if (result.Success)
                {
                    _logger.LogInformation("Email sent successfully!");
                    return Ok(new { 
                        success = true, 
                        message = "Test email sent successfully!",
                        messageId = result.MessageId
                    });
                }
                else
                {
                    _logger.LogError("Email sending failed: {Error}", result.ErrorMessage);
                    return BadRequest(new { 
                        success = false, 
                        message = "Failed to send email",
                        error = result.ErrorMessage
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending test email");
                return StatusCode(500, new { 
                    success = false, 
                    message = "Exception occurred",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost("send-booking-test")]
        public async Task<ActionResult> SendBookingTestEmail()
        {
            try
            {
                _logger.LogInformation("Starting booking email test...");

                var bookingData = new BookingConfirmationData
                {
                    OwnerEmail = "mohammadharis7704@gmail.com",
                    OwnerName = "Test Property Owner",
                    RenterName = "John Doe",
                    RenterEmail = "john@example.com",
                    PropertyName = "Test Property",
                    PropertyAddress = "123 Test Street, Test City, TC 12345",
                    CheckInDate = DateTime.Today.AddDays(1),
                    CheckOutDate = DateTime.Today.AddDays(3),
                    TotalPrice = 300.00m,
                    BookingId = 999,
                    MessageLink = "https://example.com/messages/999"
                };

                _logger.LogInformation("Sending booking confirmation email to: {Email}", bookingData.OwnerEmail);
                var result = await _emailService.SendBookingConfirmationAsync(bookingData);

                if (result.Success)
                {
                    _logger.LogInformation("Booking email sent successfully!");
                    return Ok(new { 
                        success = true, 
                        message = "Booking confirmation email sent successfully!",
                        messageId = result.MessageId
                    });
                }
                else
                {
                    _logger.LogError("Booking email sending failed: {Error}", result.ErrorMessage);
                    return BadRequest(new { 
                        success = false, 
                        message = "Failed to send booking email",
                        error = result.ErrorMessage
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending booking test email");
                return StatusCode(500, new { 
                    success = false, 
                    message = "Exception occurred",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
