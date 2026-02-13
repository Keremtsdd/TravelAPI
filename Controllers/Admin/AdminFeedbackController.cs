using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Feedback;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminFeedbackController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;
        public AdminFeedbackController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbackList = await _context.Feedbacks
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

            var result = _mapper.Map<List<FeedbackListDto>>(feedbackList);

            return Ok(new
            {
                success = true,
                message = "Öneri Ve Şikayerler",
                data = result
            });
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(int type)
        {
            var list = await _context.Feedbacks
            .Where(c => (int)c.Type == type)
            .ToListAsync();

            var result = _mapper.Map<List<FeedbackListDto>>(list);

            return Ok(new
            {
                success = true,
                message = "ID'ye Göre Öneri ve Şikayetler",
                data = result
            });
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
                return NotFound("Geri bildirim bulunamadı");

            feedback.IsRead = true;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Okundu olarak işaretlendi"
            });
        }
    }
}