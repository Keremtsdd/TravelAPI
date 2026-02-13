using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAPI.Data;
using TravelAPI.DTOs.Feedback;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Public
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public FeedbackController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            var feedback = _mapper.Map<Feedback>(dto);
            feedback.Id = Guid.NewGuid();
            feedback.CreatedAt = DateTime.UtcNow;

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