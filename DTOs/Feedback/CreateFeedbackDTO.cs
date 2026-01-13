namespace TravelAPI.DTOs.Feedback
{
    public class CreateFeedbackDTO
    {
        public int Type { get; set; }
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}