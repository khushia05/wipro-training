using System;
using System.ComponentModel.DataAnnotations;

namespace YourApp.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required, StringLength(150)]
        public string Author { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        public string Description { get; set; }
    }
}
