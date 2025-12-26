namespace TravelAPI.DTOs.Auth
{
    public class VerifyRequest
    {
        public string Email { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}