using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.Models
{
    public class PropertyType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
