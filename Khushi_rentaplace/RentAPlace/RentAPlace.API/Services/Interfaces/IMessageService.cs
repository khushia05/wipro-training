using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services.Interfaces
{
    public interface IMessageService
    {
        Task<ApiResponse<MessageDto>> SendMessageAsync(Guid senderId, MessageCreateDto createDto);
        Task<ApiResponse<List<MessageDto>>> GetMessagesByUserAsync(Guid userId);
        Task<ApiResponse<List<MessageDto>>> GetMessagesByPropertyAsync(Guid propertyId, Guid userId);
        Task<ApiResponse<MessageDto>> GetMessageByIdAsync(Guid messageId, Guid userId);
        Task<ApiResponse<bool>> MarkMessageAsReadAsync(Guid messageId, Guid userId);
        Task<ApiResponse<bool>> DeleteMessageAsync(Guid messageId, Guid userId);
    }
}
