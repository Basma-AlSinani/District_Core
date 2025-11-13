using Microsoft.AspNetCore.Mvc;
using ResidentsDatabase.DTOs;
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


        [HttpGet("list")]
        public async Task<IActionResult> GetResidentsList()
        {
            var residents = await _residentService.GetAllResidentsAsync();

            var result = residents.Select(r => new ListResidentDTO
            {
                NationalId = r.NationalId,
                FullName = $"{r.FirstName} {r.MiddleName} {r.LastName}",
                DateOfBirth = r.DateOfBirth,
                City = r.City.ToString(),
                PhoneNumber = r.PhoneNumber,
                Email = r.Email
            });

            return Ok(result);
        }
    }
}
