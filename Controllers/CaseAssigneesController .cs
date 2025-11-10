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
    [Authorize(Roles = "Admin,Investigator")]
    public class CaseAssigneesController : ControllerBase
    {
        private readonly ICaseAssigneesService _service;

        public CaseAssigneesController(ICaseAssigneesService service)
        {
            _service = service;
        }

       
        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignUserDTO dto)
        {
            // dto must include AssignerId, AssigneeId, Role
            var result = await _service.AssignUserToCaseAsync(dto.CaseId, dto.AssignerId, dto.AssigneeId, dto.Role);

            if (result)
                return BadRequest(new { message = "Assignment failed. Check assigner/assignee roles, case existence, and clearance level." });

            return CreatedAtAction(nameof(GetByCase), new { caseId = dto.CaseId }, dto);
        }

        [HttpGet("case/{caseId}")]
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
