namespace TravelAPI.DTOs.Auth
{
    public class AppUserListDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}