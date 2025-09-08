using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RentAPlace.API.Models;
using RentAPlace.API.Data;
using Microsoft.EntityFrameworkCore;

namespace RentAPlace.API.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly RentAPlaceDbContext _context;

        public SmtpEmailService(
            IOptions<EmailSettings> emailSettings,
            ILogger<SmtpEmailService> logger,
            RentAPlaceDbContext context)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _context = context;
        }

        public async Task<EmailResult> SendEmailAsync(EmailMessage message)
        {
            try
            {
                var mimeMessage = CreateMimeMessage(message);
                
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Smtp.Host, _emailSettings.Smtp.Port, _emailSettings.Smtp.EnableSsl);
                await client.AuthenticateAsync(_emailSettings.Smtp.SenderEmail, _emailSettings.Smtp.SenderPassword);
                
                var response = await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {To}. MessageId: {MessageId}", 
                    message.To, response);

                return new EmailResult
                {
                    Success = true,
                    MessageId = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", message.To);
                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<EmailResult> SendBookingConfirmationAsync(BookingConfirmationData data)
        {
            var subject = $"New Booking Confirmation - {data.PropertyName}";
            var body = GenerateBookingConfirmationHtml(data);

            var message = new EmailMessage
            {
                To = data.OwnerEmail,
                ToName = data.OwnerName,
                Subject = subject,
                Body = body,
                IsHtml = true
            };

            return await SendEmailAsync(message);
        }

        public async Task ProcessPendingNotificationsAsync()
        {
            var pendingNotifications = await _context.Notifications
                .Where(n => n.Status == "Pending" || 
                           (n.Status == "Failed" && n.RetryCount < 3 && 
                            (n.NextRetryAt == null || n.NextRetryAt <= DateTime.UtcNow)))
                .OrderBy(n => n.CreatedAt)
                .Take(10)
                .ToListAsync();

            foreach (var notification in pendingNotifications)
            {
                await ProcessNotificationAsync(notification);
            }
        }

        public async Task<EmailResult> RetryFailedNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = "Notification not found"
                };
            }

            return await ProcessNotificationAsync(notification);
        }

        private async Task<EmailResult> ProcessNotificationAsync(Notification notification)
        {
            try
            {
                var message = new EmailMessage
                {
                    To = notification.RecipientEmail,
                    Subject = notification.Subject,
                    Body = notification.Body,
                    IsHtml = true
                };

                var result = await SendEmailAsync(message);

                if (result.Success)
                {
                    notification.Status = "Sent";
                    notification.SentAt = DateTime.UtcNow;
                    notification.ErrorMessage = null;
                }
                else
                {
                    notification.RetryCount++;
                    notification.LastRetryAt = DateTime.UtcNow;
                    notification.ErrorMessage = result.ErrorMessage;

                    if (notification.RetryCount >= 3)
                    {
                        notification.Status = "Failed";
                        notification.NextRetryAt = null;
                    }
                    else
                    {
                        notification.Status = "Retrying";
                        notification.NextRetryAt = DateTime.UtcNow.AddMinutes(Math.Pow(2, notification.RetryCount));
                    }
                }

                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process notification {NotificationId}", notification.Id);
                
                notification.RetryCount++;
                notification.LastRetryAt = DateTime.UtcNow;
                notification.ErrorMessage = ex.Message;
                notification.Status = notification.RetryCount >= 3 ? "Failed" : "Retrying";
                
                if (notification.Status == "Retrying")
                {
                    notification.NextRetryAt = DateTime.UtcNow.AddMinutes(Math.Pow(2, notification.RetryCount));
                }

                await _context.SaveChangesAsync();

                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        private MimeMessage CreateMimeMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailSettings.Smtp.SenderName, _emailSettings.Smtp.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress(message.ToName ?? message.To, message.To));
            
            if (!string.IsNullOrEmpty(message.ReplyTo))
            {
                mimeMessage.ReplyTo.Add(new MailboxAddress("", message.ReplyTo));
            }

            if (message.Cc?.Any() == true)
            {
                foreach (var cc in message.Cc)
                {
                    mimeMessage.Cc.Add(new MailboxAddress("", cc));
                }
            }

            if (message.Bcc?.Any() == true)
            {
                foreach (var bcc in message.Bcc)
                {
                    mimeMessage.Bcc.Add(new MailboxAddress("", bcc));
                }
            }

            mimeMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            if (message.IsHtml)
            {
                bodyBuilder.HtmlBody = message.Body;
            }
            else
            {
                bodyBuilder.TextBody = message.Body;
            }

            if (message.Attachments?.Any() == true)
            {
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
                }
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();
            return mimeMessage;
        }

        private string GenerateBookingConfirmationHtml(BookingConfirmationData data)
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
