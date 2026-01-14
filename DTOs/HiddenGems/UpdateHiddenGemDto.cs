namespace TravelAPI.DTOs.HiddenGems
{
    public class UpdateHiddenGemDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public bool IsVerified { get; set; }
    }
}