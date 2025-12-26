namespace TravelAPI.Models.External
{
    public class RestCountry
    {
        public Name Name { get; set; } = new();
        public string Cca3 { get; set; } = string.Empty;
        public Flags Flags { get; set; } = new();
    }
    public class Name
    {
        public string Common { get; set; } = string.Empty;
    }
    public class Flags
    {
        public string Png { get; set; } = string.Empty;
    }
}