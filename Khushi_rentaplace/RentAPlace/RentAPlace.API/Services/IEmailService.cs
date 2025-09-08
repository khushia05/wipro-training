using RentAPlace.API.Models;

namespace RentAPlace.API.Services
{
    public interface IEmailService
    {
        Task<EmailResult> SendEmailAsync(EmailMessage message);
        Task<EmailResult> SendBookingConfirmationAsync(BookingConfirmationData data);
        Task ProcessPendingNotificationsAsync();
        Task<EmailResult> RetryFailedNotificationAsync(int notificationId);
    }

    public class EmailResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MessageId { get; set; }
        public int RetryCount { get; set; }
    }

    public class BookingConfirmationData
    {
        public string OwnerEmail { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string RenterName { get; set; } = string.Empty;
        public string RenterEmail { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int BookingId { get; set; }
        public string? MessageLink { get; set; }
    }
}
