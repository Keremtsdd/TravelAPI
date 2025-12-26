namespace TravelAPI.DTOs.Country
{
    public class CreateCountryDto
    {
        public string ExternalCountryId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool IsPopular { get; set; }
    }
}