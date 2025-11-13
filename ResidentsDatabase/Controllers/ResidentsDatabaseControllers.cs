using Microsoft.AspNetCore.Mvc;
using ResidentsDatabase.DTOs;
using ResidentsDatabase.Models;
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

        [HttpPost("add")]
        public async Task<IActionResult> AddResident([FromBody] AddResidentDTO newResident)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resident = new Resident
            {
                NationalId = newResident.NationalId,
                FirstName = newResident.FirstName,
                MiddleName = newResident.MiddleName,
                ThirdName = newResident.ThirdName,
                LastName = newResident.LastName,
                FullName = $"{newResident.FirstName} {newResident.MiddleName} {newResident.ThirdName} {newResident.LastName}",
                DateOfBirth = newResident.DateOfBirth,
                City = newResident.City,
                PhoneNumber = newResident.PhoneNumber,
                Email = newResident.Email
            };

            await _residentService.AddResidentAsync(resident);
            return CreatedAtAction(nameof(GetResidentsList), new { id = resident.UsersId }, resident);

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
