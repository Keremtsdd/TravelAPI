using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public enum FeedbackType
    {
        Suggestion = 1,
        Complaint = 2
    }

    public class Feedback
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public FeedbackType Type { get; set; }

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = null!;

        [Required]
        [MaxLength(5000)]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
