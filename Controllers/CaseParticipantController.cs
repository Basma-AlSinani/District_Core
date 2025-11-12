using AutoMapper;
using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Crime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CaseParticipantController : ControllerBase
    {
        private readonly ICaseParticipantService _caseParticipantService;
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;

        public CaseParticipantController(
            ICaseParticipantService caseParticipantService,
            IParticipantService participantService, IMapper mapper)
        {
            _caseParticipantService = caseParticipantService;
            _participantService = participantService;
            _mapper = mapper;
        }

        [HttpPost("AddParticipant")]
        public async Task<IActionResult> AddParticipant([FromBody] CreateParticipantDto dto)
        {
            if (dto == null)
                return BadRequest("Participant data is required");

            var result = await _participantService.CreateAsync(dto);

            if (result == null)
                return BadRequest("Failed to create participant");

            return Ok(result);
        }




        [HttpGet("GetAllCaseParticipants")]
        public async Task<IActionResult> GetAllCaseParticipants()
        {
            var data = await _caseParticipantService.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("GetCaseParticipantById/{id}")]
        public async Task<IActionResult> GetCaseParticipantById(int id)
        {
            var result = await _caseParticipantService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Case participant not found" });

            return Ok(result);
        }

        [HttpPut("UpdateCaseParticipantById/{id}")]
        public async Task<IActionResult> UpdateCaseParticipantById(int id, [FromBody] UpdateCaseParticipantDto dto)
        {
            var success = await _caseParticipantService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Case participant not found" });

            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("DeleteCaseParticipantById/{id}")]
        public async Task<IActionResult> DeleteCaseParticipantById(int id)
        {
            var success = await _caseParticipantService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Case participant not found" });

            return Ok(new { message = "Deleted successfully" });
        }
    }
}
