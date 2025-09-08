using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        public string RenterId { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CheckOutDate { get; set; }

        public int TotalNights { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

        [MaxLength(500)]
        public string? SpecialRequests { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Property Property { get; set; } = null!;

        public virtual User Renter { get; set; } = null!;

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
