using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    public class PropertyPropertyFeature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        public int PropertyFeatureId { get; set; }

        // Navigation properties
        public virtual Property Property { get; set; } = null!;

        public virtual PropertyFeature PropertyFeature { get; set; } = null!;
    }
}
