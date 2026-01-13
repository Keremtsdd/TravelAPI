using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Auth;
using TravelAPI.Generate;
using TravelAPI.Models;

namespace TravelAPI.Controllers.User
{
    [ApiController]
    [Route("auth")]
    public class RegisterController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public RegisterController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var emailExists = await _context.AppUsers.AnyAsync(x => x.Email == dto.Email);
            if (emailExists) return BadRequest("Girilen email zaten kayıtlı!");

            var user = _mapper.Map<AppUser>(dto);

            var code = OtpGenerator.Generate();
            user.Id = Guid.NewGuid();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.VerificationCode = code;
            user.CodeExpiresAt = DateTime.UtcNow.AddMinutes(5);
            user.CreatedAt = DateTime.UtcNow;

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            Console.WriteLine($"OTP CODE: {code}");

            return Ok(new { success = true, message = "Doğrulama kodu email adresinize gönderildi" });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyRequest request)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(c => c.Email == request.Email);

            if (user == null) return BadRequest("Kullanıcı Bulunamadı!");
            if (user.EmailConfirmed) return BadRequest("Bu hesap zaten doğrulanmış!");
            if (user.VerificationCode != request.Code) return BadRequest("Hatalı kod!");
            if (user.CodeExpiresAt < DateTime.UtcNow) return BadRequest("Kodun süresi dolmuş, tekrar dene");

            _mapper.Map(request, user);

            user.EmailConfirmed = true;
            user.VerificationCode = null;
            user.CodeExpiresAt = null;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Kayıt tamamlandı" });
        }

        [HttpGet("registered")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.AppUsers
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var result = _mapper.Map<List<AppUserListDto>>(users);

            return Ok(new { success = true, data = result });
        }
    }
}