using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.Favorites;
using TravelAPI.Models;

namespace TravelAPI.Controllers.User
{
    [ApiController]
    [Route("favorites")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public FavoritesController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleFavorite(FavoriteDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var ex = await _context.Favorites
            .FirstOrDefaultAsync(f => f.AppUserId == userId &&
            (f.PlaceId == dto.PlaceId && dto.PlaceId != null ||
            f.CityId == dto.CityId && dto.CityId != null ||
            f.CountryId == dto.CountryId && dto.CountryId != null));

            if (ex != null)
            {
                _context.Favorites.Remove(ex);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Favorilerden kaldırıldı", data = ex });
            }

            var favorite = _mapper.Map<Favorites>(dto);
            favorite.Id = Guid.NewGuid();
            favorite.AppUserId = userId;
            favorite.CreatedAt = DateTime.Now;

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Favorilere Eklendi", data = favorite });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFavorites()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var favorites = await _context.Favorites
            .Where(c => c.AppUserId == userId)
            .Include(c => c.Place)
            .Include(c => c.City)
            .Include(c => c.Country)
            .ToListAsync();

            return Ok(new { success = true, data = favorites });
        }
    }
}