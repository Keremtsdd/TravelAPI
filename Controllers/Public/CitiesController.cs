using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly TravelDbContext _context;
        public CitiesController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpGet("countries/{countryId}/cities")] // Ülkeye Göre Şehirleri Getirme
        public async Task<IActionResult> GetCitiesByCountry(Guid countryId)
        {
            var cities = await _context.Cities
            .Where(c => c.CountryId == countryId)
            .AsNoTracking()
            .ToListAsync();

            return Ok(cities);
        }

        [HttpGet("cities/{id}")] // Şehri ID'ye Göre Getirme
        public async Task<IActionResult> GetCitiesById(Guid id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
                return NotFound("Şehir Bulunamadı!");

            return Ok(city);
        }
    }
}