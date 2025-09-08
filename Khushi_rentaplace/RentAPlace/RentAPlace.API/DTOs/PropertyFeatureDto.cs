using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs
{
    public class PropertyFeatureDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
