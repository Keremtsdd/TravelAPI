namespace TravelAPI.DTOs.City
{
    public class CityListDto
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; } = null!;
    }
}