namespace RentAPlace.API.Models
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string? ToName { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public string? ReplyTo { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Bcc { get; set; }
        public List<EmailAttachment>? Attachments { get; set; }
    }

    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }
}
