namespace TravelAPI.DTOs.HiddenGems
{
    public class CreateHiddenGemDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Rating { get; set; }
    }
}