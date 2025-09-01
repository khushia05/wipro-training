using System.ComponentModel.DataAnnotations;
namespace TrainingApp.Models
{
    public class Trainer
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Range(1, 100)]
        public int TrainerId { get; set; }

    }
}
