using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.HiddenGems;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("api/[controller]")]
    public class HiddenGemsController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public HiddenGemsController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGems()
        {
            var gems = await _context.HiddenGems
            .Where(c => c.IsVerified)
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

            var result = _mapper.Map<List<HiddenGemsListDto>>(gems);
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGemsById(Guid id)
        {
            var gems = await _context.HiddenGems.FindAsync(id);
            if (gems == null) return BadRequest("Cevher Bulunamadı");

            var result = _mapper.Map<HiddenGemsListDto>(gems);
            return Ok(new { success = true, data = result });
        }
    }
}