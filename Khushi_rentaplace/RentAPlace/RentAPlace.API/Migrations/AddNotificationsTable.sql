-- Migration: Add Notifications Table
-- Description: Creates the Notifications table for storing email notifications and retry logic

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notifications' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[Notifications](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [RecipientEmail] [nvarchar](450) NOT NULL,
        [Subject] [nvarchar](200) NOT NULL,
        [Body] [nvarchar](max) NOT NULL,
        [Type] [nvarchar](50) NULL,
        [RelatedEntityId] [int] NULL,
        [Status] [nvarchar](50) NOT NULL DEFAULT 'Pending',
        [RetryCount] [int] NOT NULL DEFAULT 0,
        [ErrorMessage] [nvarchar](1000) NULL,
        [CreatedAt] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [SentAt] [datetime2](7) NULL,
        [LastRetryAt] [datetime2](7) NULL,
        [NextRetryAt] [datetime2](7) NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC)
    )

    -- Create indexes for better performance
    CREATE NONCLUSTERED INDEX [IX_Notifications_RecipientEmail] ON [dbo].[Notifications] ([RecipientEmail])
    CREATE NONCLUSTERED INDEX [IX_Notifications_Status] ON [dbo].[Notifications] ([Status])
    CREATE NONCLUSTERED INDEX [IX_Notifications_Type] ON [dbo].[Notifications] ([Type])
    CREATE NONCLUSTERED INDEX [IX_Notifications_CreatedAt] ON [dbo].[Notifications] ([CreatedAt])
    CREATE NONCLUSTERED INDEX [IX_Notifications_NextRetryAt] ON [dbo].[Notifications] ([NextRetryAt])

    PRINT 'Notifications table created successfully'
END
ELSE
BEGIN
    PRINT 'Notifications table already exists'
END

-- Add sample data for testing (optional)
-- INSERT INTO [dbo].[Notifications] ([RecipientEmail], [Subject], [Body], [Type], [RelatedEntityId], [Status])
-- VALUES 
-- ('owner@example.com', 'Test Notification', 'This is a test notification', 'Test', 1, 'Pending')
