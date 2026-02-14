using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class CountryWeb
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(150)]
        public string? NameTr { get; set; }

        [Required]
        [MaxLength(100)]
        public string VisaStatus { get; set; } = null!;

        [MaxLength(3000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Image { get; set; }

        [MaxLength(200)]
        public string? BestTime { get; set; }

        [MaxLength(100)]
        public string? Continent { get; set; }

        [MaxLength(100)]
        public string? PassportCategory { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
