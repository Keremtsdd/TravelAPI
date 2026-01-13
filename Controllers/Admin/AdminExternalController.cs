using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAPI.DTOs.Country;
using TravelAPI.Services;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("admin/external")]
    [Authorize(Roles = "Admin")]
    public class AdminExternalController : ControllerBase
    {
        private readonly RestCountriesService _service;
        private readonly IMapper _mapper;

        public AdminExternalController(RestCountriesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetExternalCountries()
        {
            var result = await _service.GetCountriesAsync();
            var mapResult = _mapper.Map<List<ExternalCountryDto>>(result);
            return Ok(new
            {
                success = true,
                message = "Ülkeler",
                data = mapResult
            });
        }
    }
}