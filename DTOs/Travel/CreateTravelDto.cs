namespace TravelAPI.DTOs.Travel
{
    public class CreateTravelDto
    {
        public string Title { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime? TravelDate { get; set; }
    }
}