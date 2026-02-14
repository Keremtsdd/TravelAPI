using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    public class City
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? Image { get; set; }

        public List<Place> Places { get; set; } = new();
    }
}


