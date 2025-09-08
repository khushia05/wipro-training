using RentAPlace.API.DTOs;

namespace RentAPlace.API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<ApiResponse<PagedResponse<PropertyDto>>> GetPropertiesAsync(PropertySearchDto searchDto);
        Task<ApiResponse<PropertyDto>> GetPropertyByIdAsync(Guid propertyId);
        Task<ApiResponse<PropertyDto>> CreatePropertyAsync(Guid ownerId, PropertyCreateDto createDto);
        Task<ApiResponse<PropertyDto>> UpdatePropertyAsync(Guid propertyId, Guid ownerId, PropertyUpdateDto updateDto);
        Task<ApiResponse<bool>> DeletePropertyAsync(Guid propertyId, Guid ownerId);
        Task<ApiResponse<List<PropertyDto>>> GetPropertiesByOwnerAsync(Guid ownerId);
        Task<ApiResponse<List<PropertyTypeDto>>> GetPropertyTypesAsync();
        Task<ApiResponse<List<PropertyFeatureDto>>> GetPropertyFeaturesAsync();
        Task<ApiResponse<bool>> UploadPropertyImagesAsync(Guid propertyId, Guid ownerId, List<IFormFile> images);
        Task<ApiResponse<bool>> DeletePropertyImageAsync(Guid imageId, Guid ownerId);
    }
}
