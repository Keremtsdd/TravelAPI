namespace TravelAPI.Models
{
    public class Place
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Category { get; set; }
        public bool IsPopular { get; set; }
        public double Rating { get; set; }
    }
}