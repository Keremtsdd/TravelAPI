using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs.CountryWeb;
using TravelAPI.Models;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminCountryWebController : ControllerBase
    {
        private readonly TravelDbContext _context;
        private readonly IMapper _mapper;

        public AdminCountryWebController(TravelDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ➕ Ülke Ekle
        [HttpPost]
        public async Task<IActionResult> CreateCountry(CountryWebCreateDto dto)
        {
            // DTO → ENTITY (DOĞRU)
            var country = _mapper.Map<CountryWeb>(dto);
            country.Id = Guid.NewGuid();

            _context.CountryWebs.Add(country);
            await _context.SaveChangesAsync();

            // ENTITY → DTO (response için)
            var result = _mapper.Map<CountryWebDto>(country);

            return Ok(new
            {
                success = true,
                message = "Ülke eklendi",
                data = result
            });
        }

        // ✏️ Ülke Güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(Guid id, CountryWebUpdateDto dto)
        {
            var country = await _context.CountryWebs.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
                return NotFound("Ülke bulunamadı!");

            // DTO → ENTITY
            _mapper.Map(dto, country);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<CountryWebDto>(country);

            return Ok(new
            {
                success = true,
                message = "Ülke güncellendi",
                data = result
            });
        }

        // ❌ Ülke Sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            var country = await _context.CountryWebs.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
                return NotFound("Ülke bulunamadı!");

            _context.CountryWebs.Remove(country);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Ülke silindi"
            });
        }

        // 📋 Admin – Tüm Ülkeler
        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _context.CountryWebs.ToListAsync();
            var result = _mapper.Map<List<CountryWebDto>>(countries);

            return Ok(new
            {
                success = true,
                message = "Tüm ülkeler",
                data = result
            });
        }
    }
}
