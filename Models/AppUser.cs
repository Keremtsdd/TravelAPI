namespace TravelAPI.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool EmailConfirmed { get; set; } = false;
        public string? VerificationCode { get; set; }
        public DateTime? CodeExpiresAt { get; set; }
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
