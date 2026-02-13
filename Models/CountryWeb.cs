namespace TravelAPI.Models
{
    public class CountryWeb
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string NameTr { get; set; } = null!;
        public string VisaStatus { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string BestTime { get; set; } = null!;
        public string Continent { get; set; } = null!;
        public string PassportCategory { get; set; } = null!;
        public string Notes { get; set; } = null!;
    }
}