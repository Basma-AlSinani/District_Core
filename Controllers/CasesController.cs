using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Repositories;
using CrimeManagment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrimeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CasesController : ControllerBase
    {
        private readonly ICaseService _caseService;

        public CasesController(ICaseService casesService)
        {
            _caseService = casesService;
        }

        [HttpPost("CreateNewCase")]
        [Authorize]
        public async Task<IActionResult> CreateCase([FromBody] CaseCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid case data");
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { Message = "User ID not found in token." });

                int currentUserId = int.Parse(userIdClaim);

                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(roleClaim))
                    return Unauthorized(new { Message = "User role not found in token." });

                Enum.TryParse(roleClaim, out UserRole currentUserRole);

                var newCase = await _caseService.CreateCaseAsync(dto, currentUserId, currentUserRole);
                return Ok(new
            {
                Message = "Case created successfully",
                CaseId = newCase.CaseId,
                CaseNumber = newCase.CaseNumber
            });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("updateByID/{id}")]
        public async Task<IActionResult> UpdateCase(int id, [FromBody] UpdateCaseDTO dto)
        {
            var (updatedCase, error) = await _caseService.UpdateCaseAsync(id, dto);

            if (error != null)
            {
                if (error.Contains("not found"))
                    return NotFound(new { Message = error });

                return BadRequest(new { Message = error });
            }

            return Ok(new
            {
                Message = "Case updated successfully",
                CaseId = updatedCase.CaseId
            });
        }

        // Get a list of cases with optional search
        [HttpGet("GetAllCases")]
        public async Task<IActionResult> GetCases()
        {
            var cases = await _caseService.GetCasesAsync();
            return Ok(cases);
        }

        // Get case details by ID
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetCaseDetails(int id)
        {
            {
                var caseDetails = await _caseService.GetCaseDetailsAsync(id);
                if (caseDetails == null)
                    return NotFound(new { Message = $"Case with ID {id} not found" });

                return Ok(caseDetails);
            }
        }

        [HttpDelete("DeleteCaseById{caseId}")]
        public async Task<IActionResult> DeleteCase(int caseId)
        {
            var result = await _caseService.DeleteCaseAsync(caseId);
            if (!result)
                return NotFound(new { Message = "Case not found" });

            return Ok(new { Message = "Case deleted successfully" });
        }
    }
}

