using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        public string RenterId { get; set; } = string.Empty;

        [Required]
        public int ReservationId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Property Property { get; set; } = null!;

        public virtual User Renter { get; set; } = null!;

        public virtual Reservation Reservation { get; set; } = null!;
    }
}
