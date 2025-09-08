using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public PropertyDto Property { get; set; } = null!;
        public Guid RenterId { get; set; }
        public UserDto Renter { get; set; } = null!;
        public Guid ReservationId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReviewCreateDto
    {
        [Required]
        public Guid PropertyId { get; set; }

        [Required]
        public Guid ReservationId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }

    public class ReviewUpdateDto
    {
        [Range(1, 5)]
        public int? Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
