using CrimeManagment.DTOs;
using CrimeManagment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Crime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseParticipantController : ControllerBase
    {
        private readonly ICaseParticipantService _service;

        public CaseParticipantController(ICaseParticipantService service)
        {
            _service = service;
        }

        // Add participant to case
        [HttpPost("AddParticipantToCase")]
        public async Task<IActionResult> Add([FromBody] CreateCaseParticipantDto dto)
        {
            var result = await _service.AddAsync(dto);
            if (result == null)
                return BadRequest(new { message = "Participant already linked or invalid Case/Participant ID" });

            return Ok(result);
        }

        // Get all case participants
        [HttpGet("GetAllCaseParticipants")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // Get case participant by id
        [HttpGet("GetCaseParticipantById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Case participant not found" });

            return Ok(result);
        }

        // Update case participant
        [HttpPut("UpdateCaseParticipantById/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseParticipantDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Case participant not found" });

            return Ok(new { message = "Updated successfully" });
        }

        // Delete case participant
        [HttpDelete("DeleteCaseParticipantById/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Case participant not found" });

            return Ok(new { message = "Deleted successfully" });
        }
    }
}

