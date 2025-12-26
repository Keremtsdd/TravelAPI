namespace TravelAPI.Generate
{
    public static class OtpGenerator
    {
        public static string Generate()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }
    }
}