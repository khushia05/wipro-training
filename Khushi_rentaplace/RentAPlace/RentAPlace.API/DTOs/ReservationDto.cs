using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public PropertyDto Property { get; set; } = null!;
        public Guid RenterId { get; set; }
        public UserDto Renter { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReservationCreateDto
    {
        [Required]
        public Guid PropertyId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [MaxLength(500)]
        public string? SpecialRequests { get; set; }
    }

    public class ReservationUpdateDto
    {
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string? Status { get; set; }
        public string? SpecialRequests { get; set; }
    }
}
