namespace TravelAPI.DTOs.Place
{
    public class UpdatePlaceDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; } = null!;
        public bool IsPopular { get; set; }
        public double Rating { get; set; }
    }
}