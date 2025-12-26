namespace TravelAPI.DTOs.Country
{
    public class CountryCreateDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool IsPopular { get; set; }
    }
}

