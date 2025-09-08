using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<List<NotificationDto>>> GetNotificationsByUserAsync(Guid userId);
        Task<ApiResponse<bool>> MarkNotificationAsReadAsync(Guid notificationId, Guid userId);
        Task<ApiResponse<bool>> MarkAllNotificationsAsReadAsync(Guid userId);
        Task<ApiResponse<bool>> DeleteNotificationAsync(Guid notificationId, Guid userId);
        Task<ApiResponse<bool>> CreateNotificationAsync(Guid userId, string type, string title, string message);
        Task<ApiResponse<bool>> SendEmailNotificationAsync(string toEmail, string subject, string body);
    }
}
