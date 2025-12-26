using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAPI.Services;

namespace TravelAPI.Controllers.Admin
{
    [ApiController]
    [Route("admin/external")]
    [Authorize(Roles = "Admin")]
    public class AdminExternalController : ControllerBase
    {
        private readonly RestCountriesService _service;

        public AdminExternalController(RestCountriesService service)
        {
            _service = service;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetExternalCountries()
        {
            var result = await _service.GetCountriesAsync();
            return Ok(new
            {
                success = true,
                message = "Ülkeler",
                data = result
            });
        }
    }
}