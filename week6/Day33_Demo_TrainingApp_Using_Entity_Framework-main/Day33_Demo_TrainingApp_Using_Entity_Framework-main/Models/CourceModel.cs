using System.ComponentModel.DataAnnotations;
namespace TrainingApp.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
    }
}
