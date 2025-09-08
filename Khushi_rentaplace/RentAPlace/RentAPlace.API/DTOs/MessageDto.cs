using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public PropertyDto Property { get; set; } = null!;
        public Guid SenderId { get; set; }
        public UserDto Sender { get; set; } = null!;
        public Guid ReceiverId { get; set; }
        public UserDto Receiver { get; set; } = null!;
        public string? Subject { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MessageCreateDto
    {
        [Required]
        public Guid PropertyId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [MaxLength(200)]
        public string? Subject { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; } = string.Empty;
    }

    public class MessageUpdateDto
    {
        public bool? IsRead { get; set; }
    }
}
