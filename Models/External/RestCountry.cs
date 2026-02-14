namespace TravelAPI.Models.External
{
    public class RestCountry
    {
        public RestCountryName Name { get; set; } = new();
        public string Cca3 { get; set; } = string.Empty;
        public RestCountryFlags Flags { get; set; } = new();
    }

    public class RestCountryName
    {
        public string Common { get; set; } = string.Empty;
    }

    public class RestCountryFlags
    {
        public string Png { get; set; } = string.Empty;
    }
}
