namespace TravelAPI.DTOs.Favorites
{
    public class FavoriteDto
    {
        public Guid CountryId { get; set; }
        public Guid CityId { get; set; }
        public Guid PlaceId { get; set; }
    }
}