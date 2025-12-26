namespace TravelAPI.DTOs.Place
{
    public class PlaceCardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Rating { get; set; }
    }
}