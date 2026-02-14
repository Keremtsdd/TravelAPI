using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ExternalId { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Image { get; set; }

        public bool IsPopular { get; set; } = false;

        public List<City> Cities { get; set; } = new();
    }
}


