namespace RentAPlace.API.Models
{
    public class EmailSettings
    {
        public SmtpSettings Smtp { get; set; } = new();
    }

    public class SmtpSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public string SenderName { get; set; } = "RentAPlace";
        public bool EnableSsl { get; set; } = true;
        public int TimeoutSeconds { get; set; } = 30;
    }
}
