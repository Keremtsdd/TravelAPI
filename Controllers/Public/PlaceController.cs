using AutoMapper;
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
        private readonly IMapper _mapper;

        public PlaceController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("bycity/{cityId}")] // Şehre Göre Mekanları Sırala
        public async Task<IActionResult> GetPlacesByCity(Guid cityId)
        {
            var cityExists = await _context.Cities.AnyAsync(c => c.Id == cityId);
            if (!cityExists)
                return NotFound("Şehir Bulunamadı");

            var places = await _context.Places
            .Where(c => c.CityId == cityId)
            .ToListAsync();

            var result = _mapper.Map<List<PlaceListDto>>(places);

            return Ok(result);
        }

        [HttpGet("popular")] // Popüler Mekanları Getir
        public async Task<IActionResult> PopularPlaces()
        {
            var places = await _context.Places
            .Where(p => p.IsPopular)
            .OrderByDescending(p => p.Rating)
            .Take(10)
            .AsNoTracking()
            .ToListAsync();

            var result = _mapper.Map<List<PlaceListDto>>(places);

            return Ok(result);
        }

        [HttpGet("{id}")] // ID'ye Göre Mekan Detayını Getir
        public async Task<IActionResult> GetPlaceDetail(Guid id)
        {
            var place = await _context.Places
            .Include(c => c.City)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

            if (place == null)
                return NotFound("Mekan Bulunamadı");

            var result = _mapper.Map<PlaceListDto>(place);
            return Ok(result);
        }

        [HttpGet("popularCards")] // Kart Görünümü İçin Minimal Veri
        public async Task<IActionResult> PlaceCard()
        {
            var places = await _context.Places
                .Where(p => p.IsPopular)
                .OrderByDescending(p => p.Rating)
                .Take(10)
                .AsNoTracking()
                .ToListAsync();

            var result = _mapper.Map<List<PlaceCardDto>>(places);

            return Ok(result);
        }
    }
}