using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs
{
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string PropertyType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxGuests { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int? SquareFootage { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PropertyImageDto> Images { get; set; } = new List<PropertyImageDto>();
        public List<string> Features { get; set; } = new List<string>();
        public double? AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    public class PropertyCreateDto
    {
        [Required]
        public Guid PropertyTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PricePerNight { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxGuests { get; set; } = 1;

        [Required]
        [Range(1, int.MaxValue)]
        public int Bedrooms { get; set; } = 1;

        [Required]
        [Range(1, int.MaxValue)]
        public int Bathrooms { get; set; } = 1;

        public int? SquareFootage { get; set; }

        public List<Guid> PropertyFeatureIds { get; set; } = new List<Guid>();
    }

    public class PropertyUpdateDto
    {
        [MaxLength(100)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(50)]
        public string? State { get; set; }

        [MaxLength(50)]
        public string? Country { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? PricePerNight { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxGuests { get; set; }

        [Range(1, int.MaxValue)]
        public int? Bedrooms { get; set; }

        [Range(1, int.MaxValue)]
        public int? Bathrooms { get; set; }

        public int? SquareFootage { get; set; }

        public bool? IsAvailable { get; set; }

        public List<Guid>? PropertyFeatureIds { get; set; }
    }

    public class PropertyImageDto
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class PropertySearchDto
    {
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public Guid? PropertyTypeId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? MaxGuests { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<Guid>? PropertyFeatureIds { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortOrder { get; set; } = "desc";
    }
}
