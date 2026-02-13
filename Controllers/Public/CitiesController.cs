using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.City;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;
        public CitiesController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("countries/{countryId}/cities")] // Ülkeye Göre Şehirleri Getirme
        public async Task<IActionResult> GetCitiesByCountry(Guid countryId)
        {
            var cities = await _context.Cities
            .Where(c => c.CountryId == countryId)
            .AsNoTracking()
            .ToListAsync();

            var result = _mapper.Map<List<CityListDto>>(cities);

            return Ok(result);
        }

        [HttpGet("cities/{id}")] // Şehri ID'ye Göre Getirme
        public async Task<IActionResult> GetCitiesById(Guid id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
                return NotFound("Şehir Bulunamadı!");

            var result = _mapper.Map<CityListDto>(city);

            return Ok(result);
        }
    }
}