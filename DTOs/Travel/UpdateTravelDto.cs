namespace TravelAPI.DTOs.Travel
{
    public class UpdateTravelDto
    {
        public string Title { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime? TravelDate { get; set; }
    }
}