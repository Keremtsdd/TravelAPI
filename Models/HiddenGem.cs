using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class HiddenGem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = null!;

        [MaxLength(500)]
        public string? Image { get; set; }

        // PostgreSQL için double yerine decimal önerilir (rating hassasiyeti için)
        [Range(0, 5)]
        public decimal Rating { get; set; } = 0;

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
