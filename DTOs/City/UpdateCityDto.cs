namespace TravelAPI.DTOs.City
{
    public class UpdateCityDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public string Image { get; set; } = null!;
    }
}