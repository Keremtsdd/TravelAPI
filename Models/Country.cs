namespace TravelAPI.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool IsPopular { get; set; }
        public List<City> Cities { get; set; } = new();
    }
}

