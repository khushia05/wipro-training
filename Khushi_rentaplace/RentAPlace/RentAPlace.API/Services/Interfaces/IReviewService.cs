using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ApiResponse<ReviewDto>> CreateReviewAsync(Guid renterId, ReviewCreateDto createDto);
        Task<ApiResponse<ReviewDto>> GetReviewByIdAsync(Guid reviewId);
        Task<ApiResponse<List<ReviewDto>>> GetReviewsByPropertyAsync(Guid propertyId);
        Task<ApiResponse<List<ReviewDto>>> GetReviewsByRenterAsync(Guid renterId);
        Task<ApiResponse<ReviewDto>> UpdateReviewAsync(Guid reviewId, Guid renterId, ReviewUpdateDto updateDto);
        Task<ApiResponse<bool>> DeleteReviewAsync(Guid reviewId, Guid renterId);
    }
}
