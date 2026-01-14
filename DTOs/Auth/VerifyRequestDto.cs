namespace TravelAPI.DTOs.Auth
{
    public class VerifyRequestDto
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}