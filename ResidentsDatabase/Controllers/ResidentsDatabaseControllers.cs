using Microsoft.AspNetCore.Mvc;
using ResidentsDatabase.Services;

namespace ResidentsDatabase.Controllers
{
    [Route("api/Resident")]
    [ApiController]
    public class ResidentsDatabaseControllers : ControllerBase
    {
        private readonly IResidentService _residentService;
    

    public ResidentsDatabaseControllers(IResidentService residentService)
        {
            _residentService = residentService;
        }

        [HttpGet("GetAllResidents")]
        public async Task<IActionResult> GetAllResidents()
        {
            var residents = await _residentService.GetAllResidentsAsync();
            return Ok(residents);
        }
    }
}
