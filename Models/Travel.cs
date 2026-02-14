using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class Travel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = null!;

        public DateTime? TravelDate { get; set; }

        // İsteğe bağlı: kayıt tarihi
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
