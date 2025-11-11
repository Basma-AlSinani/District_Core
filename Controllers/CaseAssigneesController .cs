using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CrimeManagment.Models;
using CrimeManagment.Services;
using CrimeManagment.DTOs;
using static CrimeManagment.DTOs.CaseAssigneesDTOs;
using System.Security.Claims;
using CrimeManagment.Repositories;

namespace CrimeManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseAssigneesController : ControllerBase
    {
        private readonly ICaseAssigneesService _service;
        private readonly IUsersRepo _usersRepo;

        public CaseAssigneesController(ICaseAssigneesService service, IUsersRepo usersRepo)
        {
            _service = service;
            _usersRepo = usersRepo;
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,Investigator")]
        public async Task<IActionResult> Assign([FromBody] AssignUserDTO dto)
        {
            int assignerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var assignee = await _usersRepo.GetByIdAsync(dto.AssigneeId);
            if (assignee == null)
                return NotFound(new { message = "Assignee not found." });

            var role = MapUserRoleToAssigneeRole(assignee.Role);

            var result = await _service.AssignUserToCaseAsync(dto.CaseId, assignerId, dto.AssigneeId, role);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return CreatedAtAction(nameof(GetByCase), new { caseId = dto.CaseId }, new { message = result.Message });
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
        private AssigneeRole MapUserRoleToAssigneeRole(UserRole userRole)
        {
            return userRole switch
            {
                UserRole.Officer => AssigneeRole.Officer,
                UserRole.Investigator => AssigneeRole.Investigator,
                UserRole.Admin => AssigneeRole.Admin,
                _ => AssigneeRole.Officer
            };
        }

    }
}