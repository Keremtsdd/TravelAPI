using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.City;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{

    [ApiController]
    [Route("admin/cities")]
    [Authorize(Roles = "Admin")]
    public class AdminCitiesController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public AdminCitiesController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost] // Şehir Ekleme
        public async Task<IActionResult> CreateCity(CityCreateDto dto)
        {
            var country = await _context.Countries.AnyAsync(c => c.Id == dto.CountryId);

            if (!country)
                return NotFound("Ülke Bulunamadı!");

            var city = new City
            {
                Id = Guid.NewGuid(),
                CountryId = dto.CountryId,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image
            };

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

            city.Name = dto.Name;
            city.Description = dto.Description;
            city.Image = dto.Image;

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
            var result = await _context.Cities
                .Where(c => c.CountryId == countryId)
                .Select(c => new CitySelectDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).OrderBy(c => c.Name)
                .ToListAsync();

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
            var result = await _context.Cities.Select(c => new CityListDto
            {
                Id = c.Id,
                CountryId = c.CountryId,
                Name = c.Name,
            }).ToListAsync();

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
            .Select(c => new CityListDto
            {
                Id = c.Id,
                CountryId = c.CountryId,
                Name = c.Name,
            }).ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Ülkeye Ait Şehirler",
                data = cities
            });
        }
    }
}
