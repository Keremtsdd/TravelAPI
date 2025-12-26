using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("countries")]
    public class CountriesController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public CountriesController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpGet] // Keşfet - Tüm Ülkeler
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _context.Countries
            .AsNoTracking()
            .ToListAsync();

            return Ok(countries);
        }

        [HttpGet("popular")] // Popüler Ülkeler
        public async Task<IActionResult> GetPopularCountries()
        {
            var popularCountries = await _context.Countries
            .Where(c => c.IsPopular)
            .AsNoTracking()
            .ToListAsync();

            return Ok(popularCountries);
        }


        [HttpGet("{id}")] // Ülke detay + Şehirler sayfası
        public async Task<IActionResult> GetById(Guid id)
        {
            var country = await _context.Countries
            .Include(c => c.Cities)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
                return NotFound();

            return Ok(country);
        }
    }
}