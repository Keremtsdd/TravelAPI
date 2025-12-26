using Microsoft.AspNetCore.Mvc;
using TravelAPI.Data;
using TravelAPI.DTOs.Feedback;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public FeedbackController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback(CreateFeedbackDTO dto)
        {
            if (!Enum.IsDefined(typeof(FeedbackType), dto.Type))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Geçersiz geri bildirim tipi"
                });
            }

            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                Type = (FeedbackType)dto.Type,
                Subject = dto.Subject,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Geri Bildiriminiz Alınmıştır",
                data = feedback
            });
        }
    }
}