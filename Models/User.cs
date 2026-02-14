using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "Admin";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
