using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Travel;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public TravelController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> CreateTravel(CreateTravelDto dto)
        {
            var travel = _mapper.Map<Travel>(dto);
            travel.Id = Guid.NewGuid();

            if (travel.TravelDate.HasValue)
            {
                travel.TravelDate = DateTime.SpecifyKind(travel.TravelDate.Value, DateTimeKind.Utc);
            }
            travel.CreatedAt = DateTime.UtcNow;

            _context.Travels.Add(travel);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Seyahat Eklendi", data = travel });
        }

        [HttpGet] // Tüm Seyahatleri Getir
        public async Task<IActionResult> GetTravels()
        {
            var travels = await _context.Travels
            .OrderByDescending(c => c.TravelDate)
            .ToListAsync();
            return Ok(new
            {
                success = true,
                message = "Tüm Seyahatler Getirildi",
                data = travels
            });
        }

        [HttpGet("{id}")] // ID'ye Göre Seyahat Getir
        public async Task<IActionResult> GetTravelsById(Guid id)
        {
            var travel = await _context.Travels.FirstOrDefaultAsync(c => c.Id == id);
            if (travel == null) return NotFound("Seyahat Bulunamadı");

            return Ok(new
            {
                success = true,
                message = "Seyahat Detayı",
                data = travel
            });
        }


        [HttpPut("{id}")] // Seyahat Güncelle
        public async Task<IActionResult> UpdateTravel(Guid id, UpdateTravelDto dto)
        {
            var travel = await _context.Travels.FirstOrDefaultAsync(c => c.Id == id);
            if (travel == null) return NotFound("Seyahat Bulunamadı");

            _mapper.Map(dto, travel);

            if (travel.TravelDate.HasValue)
            {
                travel.TravelDate = DateTime.SpecifyKind(travel.TravelDate.Value, DateTimeKind.Utc);
            }

            await _context.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                message = "Seyahat Güncellendi",
                data = travel
            });
        }

        [HttpDelete("{id}")] // Seyahat Sil
        public async Task<IActionResult> DeleteTravel(Guid id)
        {
            var travel = await _context.Travels.FindAsync(id);
            if (travel == null) return NotFound("Seyahat Bulunamadı");

            _context.Travels.Remove(travel);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Seyahat Silindi"
            });
        }
    }
}