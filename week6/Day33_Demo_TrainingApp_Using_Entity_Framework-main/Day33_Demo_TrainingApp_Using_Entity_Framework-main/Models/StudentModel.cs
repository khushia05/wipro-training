using System.ComponentModel.DataAnnotations;

namespace TrainingApp.Models
{
     public class Students
     {
          [Key]
          public int StudentId { get; set; }

          [Required]
          [StringLength(100)]
          public string Name { get; set; } = string.Empty;

          public int Age { get; set; }


     [EmailAddress]
          public string Email { get; set; } = string.Empty;         
     }
}