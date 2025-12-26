namespace TravelAPI.DTOs.City
{
    public class CityCreateDto
    {
        public Guid CountryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
    }
}

