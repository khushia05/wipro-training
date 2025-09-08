using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserDto>> RegisterAsync(UserRegistrationDto registrationDto);
        Task<ApiResponse<string>> LoginAsync(UserLoginDto loginDto);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid userId);
        Task<ApiResponse<UserDto>> UpdateUserAsync(Guid userId, UserUpdateDto updateDto);
        Task<ApiResponse<bool>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<ApiResponse<bool>> DeleteUserAsync(Guid userId);
    }
}
