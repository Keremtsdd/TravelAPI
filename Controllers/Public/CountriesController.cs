using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Country;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("countries")]
    public class CountriesController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public CountriesController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet] // Keşfet - Tüm Ülkeler
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _context.Countries
            .AsNoTracking()
            .ToListAsync();

            var result = _mapper.Map<List<CountrySelectDto>>(countries);

            return Ok(result);
        }

        [HttpGet("popular")] // Popüler Ülkeler
        public async Task<IActionResult> GetPopularCountries()
        {
            var popularCountries = await _context.Countries
            .Where(c => c.IsPopular)
            .AsNoTracking()
            .ToListAsync();

            var result = _mapper.Map<List<CountrySelectDto>>(popularCountries);

            return Ok(result);
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

            var result = _mapper.Map<CountrySelectDto>(country);

            return Ok(result);
        }
    }
}