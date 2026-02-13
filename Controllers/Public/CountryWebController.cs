using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.CountryWeb;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryWebController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public CountryWebController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 🌍 Public – Tüm Ülkeler
        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _context.CountryWebs
                .AsNoTracking()
                .ToListAsync();

            var result = _mapper.Map<List<CountryWebDto>>(countries);
            return Ok(result);
        }

        // 🌍 Public – Ülke Detay
        [HttpGet("countries/{id}")]
        public async Task<IActionResult> GetCountryById(Guid id)
        {
            var country = await _context.CountryWebs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (country == null)
                return NotFound("Ülke bulunamadı!");

            var result = _mapper.Map<CountryWebDto>(country);
            return Ok(result);
        }

        // 🌍 Public – Kıtaya Göre Ülkeler
        [HttpGet("continents/{continent}/countries")]
        public async Task<IActionResult> GetCountriesByContinent(string continent)
        {
            var countries = await _context.CountryWebs
                .Where(x => x.Continent == continent)
                .AsNoTracking()
                .ToListAsync();

            var result = _mapper.Map<List<CountryWebDto>>(countries);
            return Ok(result);
        }
    }
}
