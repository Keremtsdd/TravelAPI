namespace TravelAPI.Models
{
    public class HiddenGem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}