namespace TravelAPI.DTOs.Feedback
{
    public class FeedbackListDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}