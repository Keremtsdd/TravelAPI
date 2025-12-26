namespace TravelAPI.DTOs.Place
{
    public class PlaceListDto
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPopular { get; set; }
        public double Rating { get; set; }
    }
}