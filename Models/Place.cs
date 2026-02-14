using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    public class Place
    {
        [Key]
        public Guid Id { get; set; }

        // Foreign Key
        public Guid CityId { get; set; }
        public City City { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = null!;

        [MaxLength(500)]
        public string? Image { get; set; }

        // Latitude & Longitude için decimal önerilir
        [Column(TypeName = "numeric(9,6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "numeric(9,6)")]
        public decimal Longitude { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = "General";

        public bool IsPopular { get; set; } = false;

        // Rating için decimal
        [Column(TypeName = "numeric(2,1)")]
        public decimal Rating { get; set; } = 0;
    }
}
