namespace TravelAPI.DTOs.Country
{
    public class UpdateCountryDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool IsPopular { get; set; }
    }
}