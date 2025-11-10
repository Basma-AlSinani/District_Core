using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CrimeManagment.Models;
using CrimeManagment.Services;
using CrimeManagment.DTOs;
using static CrimeManagment.DTOs.CaseAssigneesDTOs;

namespace CrimeManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseAssigneesController : ControllerBase
    {
        private readonly ICaseAssigneesService _service;

        public CaseAssigneesController(ICaseAssigneesService service)
        {
            _service = service;
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,Investigator")]
        public async Task<IActionResult> Assign([FromBody] AssignUserDTO dto)
        {
            var (success, message) = await _service.AssignUserToCaseAsync(dto.CaseId, dto.AssignerId, dto.AssigneeId, dto.Role);

            if (!success)
                return BadRequest(new { message });

            return CreatedAtAction(nameof(GetByCase), new { caseId = dto.CaseId }, new { message });
        }


        [HttpGet("case/{caseId}")]
        [Authorize(Roles = "Admin,Investigator")]
        public async Task<IActionResult> GetByCase(int caseId)
        {
            var list = await _service.GetAssigneesByCaseIdAsync(caseId);
            if (!list.Any())
                return NotFound(new { message = "No assignees found for this case." });
            return Ok(list);
        }

        [HttpPut("{id}/progress")]
        [Authorize(Roles = "Admin,Officer,Investigator")]
        public async Task<IActionResult> UpdateProgress(int id, [FromQuery] ProgreessStatus newStatus)
        {
            var result = await _service.UpdateAssigneeStatusAsync(id, newStatus);
            if (!result)
                return NotFound(new { message = "Assignee not found." });

            return Ok(new { message = "Assignee status updated successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Investigator")]
        public async Task<IActionResult> Remove(int id)
        {
            var assignee = await _service.GetByIdAsync(id);
            if (assignee == null)
                return NotFound(new { message = "Assignee not found." });

            await _service.DeleteAsync(id);
            return Ok(new { message = "Assignee removed successfully." });
        }
    }
}