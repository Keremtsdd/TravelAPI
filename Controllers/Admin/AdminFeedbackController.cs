using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Feedback;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("admin/feedback")]
    [Authorize(Roles = "Admin")]
    public class AdminFeedbackController : ControllerBase
    {
        private readonly TravelDbContext _context;
        public AdminFeedbackController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var feedbackList = await _context.Feedbacks
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new FeedbackListDto
            {
                Id = c.Id,
                Type = c.Type.ToString(),
                Subject = c.Subject,
                IsRead = c.IsRead,
                CreatedAt = c.CreatedAt,
            }).ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Öneri Ve Şikayerler",
                data = feedbackList
            });
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(int type)
        {
            var list = await _context.Feedbacks
            .Where(c => (int)c.Type == type)
            .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "ID'ye Göre Öneri ve Şikayetler",
                data = list
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