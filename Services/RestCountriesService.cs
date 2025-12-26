using System.Text.Json;
using TravelAPI.DTOs.Country;
using TravelAPI.Models.External;

namespace TravelAPI.Services
{
    public class RestCountriesService
    {
        private readonly HttpClient _httpClient;

        public RestCountriesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExternalCountryDto>> GetCountriesAsync()
        {
            var response = await _httpClient.GetAsync(
                "v3.1/all?fields=name,cca3,flags"
            );

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var countries = JsonSerializer.Deserialize<List<RestCountry>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return countries!
                .Select(c => new ExternalCountryDto
                {
                    Id = c.Cca3,
                    Name = c.Name.Common,
                    Flags = c.Flags?.Png ?? ""
                })
                .OrderBy(c => c.Name)
                .ToList();
        }
    }
}
