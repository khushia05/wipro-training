# Email Notification System Setup

This document provides instructions for setting up the email notification system for RentAPlace.

## Overview

The email notification system sends booking confirmation emails to property owners when a new booking is created. It includes:

- Gmail SMTP integration using MailKit
- Retry logic with exponential backoff
- Database logging of email status
- Angular notification badge component
- Unit tests for email service

## Prerequisites

1. .NET 8.0 SDK
2. SQL Server
3. Gmail account with App Password enabled
4. Angular 17+ (for frontend components)

## Setup Instructions

### 1. Gmail App Password Setup

1. Go to your Google Account settings
2. Navigate to Security → 2-Step Verification
3. At the bottom, select "App passwords"
4. Generate a new app password for "Mail"
5. Copy the 16-character password (e.g., `vvhbdyozgycvntta`)

### 2. Environment Configuration

#### Option A: Using appsettings.json (Development)
```json
{
  "EmailSettings": {
    "Smtp": {
      "Host": "smtp.gmail.com",
      "Port": 587,
      "SenderEmail": "your-email@gmail.com",
      "SenderPassword": "your-16-character-app-password",
      "SenderName": "RentAPlace",
      "EnableSsl": true,
      "TimeoutSeconds": 30
    }
  }
}
```

#### Option B: Using .NET Secret Manager (Recommended for Production)
```bash
# Install the secret manager tool
dotnet tool install --global dotnet-user-secrets

# Set secrets
dotnet user-secrets set "EmailSettings:Smtp:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:Smtp:SenderPassword" "your-16-character-app-password"
```

#### Option C: Using Environment Variables
```bash
# Windows
set EmailSettings__Smtp__SenderEmail=your-email@gmail.com
set EmailSettings__Smtp__SenderPassword=your-16-character-app-password

# Linux/Mac
export EmailSettings__Smtp__SenderEmail=your-email@gmail.com
export EmailSettings__Smtp__SenderPassword=your-16-character-app-password
```

### 3. Database Migration

Run the SQL migration script to create the Notifications table:

```sql
-- Execute the migration script
-- File: Migrations/AddNotificationsTable.sql
```

Or use Entity Framework migrations:
```bash
dotnet ef migrations add AddNotificationsTable
dotnet ef database update
```

### 4. NuGet Package Installation

The required MailKit package is already included in the project file:
```xml
<PackageReference Include="MailKit" Version="4.3.0" />
```

### 5. Service Registration

The email service is already registered in `Program.cs`:
```csharp
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
```

## Usage

### Backend API

#### Send Booking Confirmation Email
```csharp
// This happens automatically when a booking is created via POST /api/bookings/simple
// The system will:
// 1. Create the booking
// 2. Send email to property owner
// 3. Store notification record for retry if email fails
```

#### Manual Email Sending
```csharp
// Inject IEmailService in your controller
public class MyController : ControllerBase
{
    private readonly IEmailService _emailService;
    
    public MyController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public async Task<IActionResult> SendEmail()
    {
        var message = new EmailMessage
        {
            To = "recipient@example.com",
            Subject = "Test Email",
            Body = "This is a test email",
            IsHtml = true
        };
        
        var result = await _emailService.SendEmailAsync(message);
        return Ok(result);
    }
}
```

#### Process Pending Notifications
```csharp
// This can be called periodically to retry failed emails
await _emailService.ProcessPendingNotificationsAsync();
```

### Frontend Angular

#### Add Notification Badge to Header
```html
<!-- In your header component -->
<app-notification-badge></app-notification-badge>
```

#### Use Notification Service
```typescript
// In your component
constructor(private notificationService: NotificationService) {}

ngOnInit() {
  // Get notifications
  this.notificationService.getNotifications().subscribe(notifications => {
    console.log(notifications);
  });
  
  // Get notification stats
  this.notificationService.getNotificationStats().subscribe(stats => {
    console.log('Unread notifications:', stats.unreadCount);
  });
}
```

## API Endpoints

### Notifications Controller
- `GET /api/notifications` - Get user notifications
- `GET /api/notifications/stats` - Get notification statistics
- `POST /api/notifications` - Create new notification
- `PUT /api/notifications/{id}/mark-read` - Mark notification as read
- `DELETE /api/notifications/{id}` - Delete notification
- `POST /api/notifications/{id}/retry` - Retry failed notification (Admin only)

## Testing

### Unit Tests
Run the email service unit tests:
```bash
dotnet test --filter "EmailServiceTests"
```

### Integration Testing
1. Create a test booking via POST /api/bookings/simple
2. Check the Notifications table for email records
3. Verify email is received by property owner

## Monitoring and Troubleshooting

### Check Email Status
```sql
-- View all notifications
SELECT * FROM Notifications ORDER BY CreatedAt DESC;

-- View failed notifications
SELECT * FROM Notifications WHERE Status = 'Failed';

-- View pending notifications
SELECT * FROM Notifications WHERE Status = 'Pending';
```

### Logs
Check application logs for email-related messages:
- Email sent successfully
- Email sending failures
- Notification retry attempts

### Common Issues

1. **Authentication Failed**: Check Gmail App Password
2. **Connection Timeout**: Verify SMTP settings and network connectivity
3. **Email Not Received**: Check spam folder, verify recipient email
4. **Database Errors**: Ensure Notifications table exists and is accessible

## Security Considerations

1. **Never commit email passwords to version control**
2. **Use environment variables or secret manager for production**
3. **Implement rate limiting for email sending**
4. **Consider using a dedicated email service (SendGrid, AWS SES) for production**

## Production Recommendations

1. **Use a dedicated email service** instead of Gmail SMTP
2. **Implement email templates** for better formatting
3. **Add email analytics** and delivery tracking
4. **Set up monitoring** for failed email notifications
5. **Implement email queuing** for high-volume scenarios

## Support

For issues or questions:
1. Check the logs for error messages
2. Verify SMTP configuration
3. Test with a simple email first
4. Check database connectivity
