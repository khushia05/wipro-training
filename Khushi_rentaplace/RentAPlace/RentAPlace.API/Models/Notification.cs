using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(450)]
        public string RecipientEmail { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty; // "BookingConfirmation", "BookingCancellation", etc.
        
        public int? RelatedEntityId { get; set; } // Booking ID, Property ID, etc.
        
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed, Retrying
        
        public int RetryCount { get; set; } = 0;
        
        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? SentAt { get; set; }
        
        public DateTime? LastRetryAt { get; set; }
        
        public DateTime? NextRetryAt { get; set; }
    }
}