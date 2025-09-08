using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ApiResponse<ReservationDto>> CreateReservationAsync(Guid renterId, ReservationCreateDto createDto);
        Task<ApiResponse<ReservationDto>> GetReservationByIdAsync(Guid reservationId);
        Task<ApiResponse<List<ReservationDto>>> GetReservationsByRenterAsync(Guid renterId);
        Task<ApiResponse<List<ReservationDto>>> GetReservationsByOwnerAsync(Guid ownerId);
        Task<ApiResponse<ReservationDto>> UpdateReservationStatusAsync(Guid reservationId, Guid ownerId, string status);
        Task<ApiResponse<bool>> CancelReservationAsync(Guid reservationId, Guid userId);
        Task<ApiResponse<bool>> CheckAvailabilityAsync(Guid propertyId, DateTime checkInDate, DateTime checkOutDate);
    }
}
