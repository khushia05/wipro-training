namespace RentAPlace.API.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? RelatedEntityId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? LastRetryAt { get; set; }
        public DateTime? NextRetryAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class CreateNotificationDto
    {
        public string RecipientEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? RelatedEntityId { get; set; }
    }

    public class NotificationStatsDto
    {
        public int TotalNotifications { get; set; }
        public int UnreadCount { get; set; }
        public int PendingCount { get; set; }
        public int FailedCount { get; set; }
    }
}