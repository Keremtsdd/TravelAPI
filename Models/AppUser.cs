using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    [Table("app_users")]
    public class AppUser
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = null!;

        public bool EmailConfirmed { get; set; } = false;

        [MaxLength(20)]
        public string? VerificationCode { get; set; }

        public DateTime? CodeExpiresAt { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
