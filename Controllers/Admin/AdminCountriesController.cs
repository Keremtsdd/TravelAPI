using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Country;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("admin/countries")]
    [Authorize(Roles = "Admin")]
    public class AdminCountriesController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public AdminCountriesController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost] // Ülke Ekle
        public async Task<IActionResult> CreateCountry(CreateCountryDto dto)
        {
            var exists = await _context.Countries
            .AnyAsync(c => c.ExternalId == dto.ExternalCountryId);

            if (exists)
                return BadRequest("Bu Ülke Zaten Mevcut!");

            var country = new Country
            {
                Id = Guid.NewGuid(),
                ExternalId = dto.ExternalCountryId,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                IsPopular = dto.IsPopular,

            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Ülke Eklendi",
                data = country
            });
        }

        [HttpGet] // Tüm Ülkeleri Listele
        public async Task<IActionResult> GetAll()
        {
            var countries = await _context.Countries
            .OrderBy(c => c.Name)
            .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Tüm Ülkeler",
                data = countries
            });
        }

        [HttpGet("{id}")] // ID'ye Göre Ülke Getir
        public async Task<IActionResult> GetById(Guid id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
                return NotFound("Ülke bulunamadı");

            return Ok(new
            {
                success = true,
                message = "ID'ye Göre Ülkeler",
                data = country
            });
        }

        [HttpPut("{id}")] // Ülke Güncelle
        public async Task<IActionResult> UpdateCountry(Guid id, UpdateCountryDto dto)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
                return NotFound("Ülke Bulunamadı!");

            country.Name = dto.Name;
            country.Description = dto.Description;
            country.Image = dto.Image;
            country.IsPopular = dto.IsPopular;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Ülke Güncellendi",
                data = country
            });
        }

        [HttpDelete("{id}")] // Ülke Sil
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
                return NotFound("Ülke Bulunamadı!");

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Şehir Silindi"
            });

        }

        [HttpGet("select")] // Admin Dropdown için Ülkeler
        public async Task<IActionResult> GetCountriesForSelect()
        {
            var result = await _context.Countries.Select(c => new CountrySelectDto
            {
                Id = c.Id,
                Name = c.Name,
            }).OrderBy(c => c.Name).ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Admin İçin Ülkeler",
                data = result
            });
        }
    }
}

