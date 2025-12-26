using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Place;


namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("places")]
    public class PlaceController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public PlaceController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpGet("bycity/{cityId}")] // Şehre Göre Mekanları Sırala
        public async Task<IActionResult> GetPlacesByCity(Guid cityId)
        {
            var result = await _context.Cities.AnyAsync(c => c.Id == cityId);
            if (!result)
                return NotFound("Şehir Bulunamadı");

            var places = await _context.Places
            .Where(c => c.CityId == cityId)
            .ToListAsync();

            return Ok(places);
        }

        [HttpGet("popular")] // Popüler Mekanları Getir
        public async Task<IActionResult> PopularPlaces()
        {
            var places = await _context.Places
            .Where(p => p.IsPopular)
            .OrderByDescending(p => p.Rating)
            .Take(10)
            .ToListAsync();

            return Ok(places);
        }

        [HttpGet("{id}")] // ID'ye Göre Mekan Detayını Getir
        public async Task<IActionResult> GetPlaceDetail(Guid id)
        {
            var place = await _context.Places
            .Include(c => c.City)
            .FirstOrDefaultAsync(c => c.Id == id);

            if (place == null)
                return NotFound("Mekan Bulunamadı");

            return Ok(place);
        }

        [HttpGet("popularCards")]
        public async Task<IActionResult> PlaceCard()
        {
            var places = await _context.Places
                .Where(p => p.IsPopular)
                .OrderByDescending(p => p.Rating)
                .Take(10)
                .Select(p => new PlaceCardDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Image = p.Image,
                    Rating = p.Rating
                })
                .ToListAsync();

            return Ok(places);
        }
    }
}