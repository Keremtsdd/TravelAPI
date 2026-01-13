using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Auth;
using TravelAPI.Services;

namespace TravelAPI.Controllers.User
{
    [ApiController]
    [Route("auth")]
    public class LoginController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public LoginController(TravelDbContext context, JwtService service, IMapper mapper)
        {
            _context = context;
            _jwtService = service;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (user == null)
                return BadRequest("Email veya Şifre hatalı!");

            var passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordOk)
                return BadRequest("Email veya Şifre hatalı!");

            if (!user.EmailConfirmed)
                return BadRequest("Email doğrulanmamış!");

            var token = _jwtService.GenerateAppUserToken(user);

            var userDto = _mapper.Map<AppUserListDto>(user);

            return Ok(new
            {
                success = true,
                token = token,
                user = userDto
            });
        }

        [Authorize]
        [HttpDelete("deleteaccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

            var user = await _context.AppUsers
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user == null)
                return NotFound("Kullanıcı Bulunamadı");

            _context.AppUsers.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Hesap başarıyla silindi"
            });
        }
    }
}