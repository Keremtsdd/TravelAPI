using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Place;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminPlaceController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public AdminPlaceController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost] // Mekan Ekle
        public async Task<IActionResult> CreatePlace(CreatePlaceDto dto)
        {
            var cityExists = await _context.Cities.AnyAsync(c => c.Id == dto.CityId);

            if (!cityExists)
                return NotFound("Şehir Bulunamadı!");

            var place = _mapper.Map<Place>(dto);
            place.Id = Guid.NewGuid();

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

            _mapper.Map(dto, place);

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
            var cityExists = await _context.Places.AnyAsync(c => c.CityId == cityId);
            if (!cityExists)
                return NotFound("Şehir Bulunamadı!");

            var places = await _context.Places
           .Where(p => p.CityId == cityId)
           .ToListAsync();

            var result = _mapper.Map<List<PlaceListDto>>(places);
            return Ok(new
            {
                success = true,
                message = "ID'ye Göre Mekanlar",
                data = result
            });
        }
    }
}