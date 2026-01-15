namespace TravelAPI.Models
{
    public class Favorites
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public Guid PlaceId { get; set; }
        public Place Place { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}