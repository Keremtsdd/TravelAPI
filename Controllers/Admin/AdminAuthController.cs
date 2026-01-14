using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Admin;
using TravelAPI.Services;

[ApiController]
[Route("admin/auth")]
public class AdminAuthController : ControllerBase
{
    private readonly TravelDbContext _context;
    private readonly JwtService _jwtService;

    public AdminAuthController(TravelDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AdminLoginDto dto)
    {
        var admin = await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == dto.UserName);

        if (admin == null)
            return Unauthorized("Hatalı KullanıcıAdı veya Şifre");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash))
            return Unauthorized("Hatalı KullanıcıAdı veya Şifre");

        var token = _jwtService.GenerateAdminToken(admin);

        return Ok(new { token, userName = admin.UserName, role = admin.Role });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var admin = await _context.Users.FindAsync(Guid.Parse(userId!));

        if (admin == null)
            return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, admin.PasswordHash))
            return BadRequest("Eski şifre yanlış");

        admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _context.SaveChangesAsync();

        return Ok("Şifre başarıyla değiştirildi.");
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new
        {
            success = true,
            message = "Çıkış Yapıldı."
        });
    }
}
