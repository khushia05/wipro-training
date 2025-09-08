using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace RentAPlace.API
{
    public class EmailTestConsole
    {
        public static async Task TestEmail()
        {
            try
            {
                Console.WriteLine("Testing Gmail SMTP connection...");
                
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("RentAPlace", "mohammadharis7704@gmail.com"));
                message.To.Add(new MailboxAddress("Test User", "mohammadharis7704@gmail.com"));
                message.Subject = "Test Email from RentAPlace Console";
                message.Body = new TextPart("plain")
                {
                    Text = "This is a test email sent from the RentAPlace console application to verify Gmail SMTP is working."
                };

                using var client = new SmtpClient();
                
                Console.WriteLine("Connecting to Gmail SMTP server...");
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                
                Console.WriteLine("Authenticating with Gmail...");
                await client.AuthenticateAsync("mohammadharis7704@gmail.com", "vvhbdyozgycvntta");
                
                Console.WriteLine("Sending email...");
                var response = await client.SendAsync(message);
                
                Console.WriteLine($"Email sent successfully! Response: {response}");
                
                await client.DisconnectAsync(true);
                Console.WriteLine("Disconnected from SMTP server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
