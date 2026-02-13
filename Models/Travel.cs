namespace TravelAPI.Models
{
    public class Travel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime? TravelDate { get; set; }
    }
}