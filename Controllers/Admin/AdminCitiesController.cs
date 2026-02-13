using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.City;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminCitiesController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public AdminCitiesController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost] // Şehir Ekleme
        public async Task<IActionResult> CreateCity(CityCreateDto dto)
        {
            var country = await _context.Countries.AnyAsync(c => c.Id == dto.CountryId);

            if (!country)
                return NotFound("Ülke Bulunamadı!");

            var city = _mapper.Map<City>(dto);
            city.Id = Guid.NewGuid();

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Şehir Eklendi",
                data = city
            });
        }

        [HttpPut("{id}")] // Şehir Güncelleme
        public async Task<IActionResult> UpdateCity(Guid id, UpdateCityDto dto)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
                return NotFound("Şehir Bulunamadı!");

            _mapper.Map(dto, city);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                message = "Şehir Güncellendi",
                data = city
            });
        }

        [HttpDelete("{id}")] // Şehir Silme
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);

            if (city == null)
                return NotFound("Şehir Bulunamadı!");

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Şehir Silindi"
            });

        }

        [HttpGet("select/{countryId}")] // Ülkeye Ait Şehirleri Getirme (Dropdown İçin)
        public async Task<IActionResult> GetCitiesForSelect(Guid countryId)
        {
            var cities = await _context.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var result = _mapper.Map<List<CitySelectDto>>(cities);

            return Ok(new
            {
                success = true,
                message = "Ülkeye Ait Şehirler",
                data = result
            });
        }

        [HttpGet] // Tüm Şehirleri Getirme
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _context.Cities.ToListAsync();

            var result = _mapper.Map<List<CityListDto>>(cities);

            return Ok(new
            {
                success = true,
                message = "Tüm Şehirler",
                data = result
            });
        }

        [HttpGet("bycountry/{countryId}")] // Ülkeye Ait Şehirleri Getirme
        public async Task<IActionResult> GetByCountry(Guid countryId)
        {
            var countryExists = await _context.Countries.AnyAsync(c => c.Id == countryId);

            if (!countryExists)
                return NotFound("Ülke Bulunamadı");

            var cities = await _context.Cities
            .Where(c => c.CountryId == countryId)
            .ToListAsync();

            var result = _mapper.Map<List<CityListDto>>(cities);

            return Ok(new
            {
                success = true,
                message = "Ülkeye Ait Şehirler",
                data = result
            });
        }
    }
}
