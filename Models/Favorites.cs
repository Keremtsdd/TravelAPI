using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    public class Favorites
    {
        [Key]
        public Guid Id { get; set; }

        // 🔗 User Relation
        [Required]
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;

        // 🔗 Country Relation
        public Guid? CountryId { get; set; }
        public Country? Country { get; set; }

        // 🔗 City Relation
        public Guid? CityId { get; set; }
        public City? City { get; set; }

        // 🔗 Place Relation
        public Guid? PlaceId { get; set; }
        public Place? Place { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
