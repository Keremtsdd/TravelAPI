using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Auth;
using TravelAPI.Generate;
using TravelAPI.Models;

namespace TravelAPI.Controllers.User
{
    [ApiController]
    [Route("auth/register")]
    public class RegisterController : ControllerBase
    {
        private readonly TravelDbContext _context;

        public RegisterController(TravelDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var email = await _context.AppUsers
            .AnyAsync(x => x.Email == dto.Email);

            if (email) return BadRequest("Girilen email zaten kayıtlı!");

            var code = OtpGenerator.Generate();

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                EmailConfirmed = false,
                VerificationCode = code,
                CodeExpiresAt = DateTime.UtcNow.AddMinutes(5),
                CreatedAt = DateTime.UtcNow
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            Console.WriteLine($"OTP CODE: {code}");

            return Ok(new
            {
                success = true,
                message = "Doğrulama kodu email adresinize gönderildi"
            });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyRequest request)
        {
            var user = await _context.AppUsers
            .FirstOrDefaultAsync(c => c.Email == request.Email);

            if (user == null)
                return BadRequest("Kullanıcı Bulunamadı!");
            if (user.EmailConfirmed)
                return BadRequest("Bu hesap zaten doğrulanmış!");
            if (user.VerificationCode != request.Code)
                return BadRequest("Hatalı kod!");
            if (user.CodeExpiresAt < DateTime.UtcNow)
                return BadRequest("Kodun süresi dolmuş,tekrar dene");

            user.Name = request.Name;
            user.BirthDate = request.BirthDate;
            user.EmailConfirmed = true;
            user.VerificationCode = null;
            user.CodeExpiresAt = null;

            await _context.SaveChangesAsync();

            return Ok("Kayıt Tamamlandı!");
        }
    }
}