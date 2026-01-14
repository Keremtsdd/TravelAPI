using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAPI.Data;
using TravelAPI.DTOs.HiddenGems;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("admin/hiddengems")]
    [Authorize(Roles = "Admin")]
    public class AdminHiddenGemsController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public AdminHiddenGemsController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGem(CreateHiddenGemDto dto)
        {
            var gem = _mapper.Map<HiddenGem>(dto);
            gem.Id = Guid.NewGuid();
            gem.CreatedAt = DateTime.Now;
            gem.IsVerified = true;

            _context.HiddenGems.Add(gem);
            await _context.SaveChangesAsync();

            return Ok(new { succes = true, message = "Saklı Cevher Eklendi", data = gem });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGem(Guid id, UpdateHiddenGemDto dto)
        {
            var gem = await _context.HiddenGems.FindAsync(id);
            if (gem == null)
                return BadRequest("Saklı Cevher Bulunamadı!");

            _mapper.Map(dto, gem);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Saklı Cevher Güncellendi", data = gem });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGem(Guid id)
        {
            var gem = await _context.HiddenGems.FindAsync(id);
            if (gem == null) return BadRequest("Silinecek Cevher Bulunamadı,Kayıtlı Id Giriniz!");

            _context.HiddenGems.Remove(gem);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Silindi" });
        }
    }
}