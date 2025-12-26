using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Place;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("admin/places")]
    [Authorize(Roles = "Admin")]
    public class AdminPlaceController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public AdminPlaceController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost] // Mekan Ekle
        public async Task<IActionResult> CreatePlace(CreatePlaceDto dto)
        {
            var city = await _context.Cities.AnyAsync(c => c.Id == dto.CityId);

            if (!city)
                return NotFound("Şehir Bulunamadı!");

            var place = new Place
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                Longitude = dto.Longitude,
                Latitude = dto.Latitude,
                Category = dto.Category,
                IsPopular = dto.IsPopular,
                Rating = dto.Rating,
                CityId = dto.CityId
            };
            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Mekan Eklendi",
                data = place
            });
        }

        [HttpPut("{id}")] // Mekan Güncelle
        public async Task<IActionResult> UpdatePlace(Guid id, UpdatePlaceDto dto)
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
                return NotFound("Mekan Bulunamadı!");

            place.Name = dto.Name;
            place.Description = dto.Description;
            place.Image = dto.Image;
            place.Longitude = dto.Longitude;
            place.Latitude = dto.Latitude;
            place.Category = dto.Category;
            place.IsPopular = dto.IsPopular;
            place.Rating = dto.Rating;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Mekan Güncellendi",
                data = place
            });
        }

        [HttpDelete("{id}")] // Mekan Sil
        public async Task<IActionResult> DeletePlace(Guid id)
        {
            var place = await _context.Places.FindAsync(id);

            if (place == null)
                return NotFound("Mekan Bulunamadı");

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Mekan Silindi"
            });

        }

        [HttpGet("bycity/{cityId}")] // Şehre Göre Mekanları Sırala
        public async Task<IActionResult> GetByCityId(Guid cityId)
        {
            var place = await _context.Places.AnyAsync(c => c.CityId == cityId);
            if (!place)
                return NotFound("Şehir Bulunamadı!");

            var result = await _context.Places
           .Where(p => p.CityId == cityId)
           .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "ID'ye Göre Mekanlar",
                data = place
            });
        }
    }
}