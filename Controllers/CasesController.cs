using Crime.DTOs;
using Crime.Models;
using Crime.Repositories;
using Crime.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{
    [Route("api/Cases")]
    [ApiController]

    public class CasesController : ControllerBase
    {
        private readonly ICaseService _caseService;

        public CasesController(ICaseService casesService)
        {
            _caseService = casesService;
        }

        [HttpPost("Submit Crime Report")]
        public async Task<IActionResult> SubmitCrimeReport([FromBody] CrimeReportCreateDTO dto)
        {
            var report = await _caseService.SubmitCrimeReportAsync(dto);
            return Ok(new
            {
                Message = "Crime report submitted successfully",
                CrimeReportId = report.CrimeReportId
            });
        }

        [HttpPost("Create New Case")]
        public async Task<IActionResult> CreateCase([FromBody] CaseCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid case data");

            var newCase = await _caseService.CreateCaseAsync(dto);

            return Ok(new
            {
                Message = "Case created successfully",
                CaseId = newCase.CaseId,
                CaseNumber = newCase.CaseNumber
            });
        }

        [HttpPut("update By ID/{id}")]
        public async Task<IActionResult> UpdateCase(int id, [FromBody] UpdateCaseDTO dto)
        {
            var updatedCase = await _caseService.UpdateCaseAsync(id, dto);
            return Ok(new
            {
                Message = "Case updated successfully",
                CaseId = updatedCase.CaseId
            });
        }

        // Get a list of cases with optional search
        [HttpGet("Get All Cases")]
        public async Task<IActionResult> GetCases([FromQuery] string? search)
        {
            var cases = await _caseService.GetCasesAsync(search);
            return Ok(cases);
        }

        // Get case details by ID
        [HttpGet("Get By ID/{id}")]
        public async Task<IActionResult> GetCaseDetails(int id)
        {
            {
                var caseDetails = await _caseService.GetCaseDetailsAsync(id);
                return Ok(caseDetails);
            }
        }

        // get All Assignees by ID
        [HttpGet("Get/All/Assignees/{caseId}")]
        public async Task<IActionResult> GetAllAssignees(int caseId)
        {
            var result = await _caseService.GetAssigneesByCaseIdAsync(caseId);
            if (!result.Any()) return NotFound(new { message = "No assignees found for this case." });
            return Ok(result);
        }

        // get All Evidence by ID
        [HttpGet("Get/All/Evidence/{caseId}")]
        public async Task<IActionResult> GetAllEvidence(int caseId)
        {
            var result = await _caseService.GetEvidenceByCaseIdAsync(caseId);
            if (!result.Any()) return NotFound(new { message = "No evidences found for this case." });
            return Ok(result);
        }

        // get All Suspects by ID
        [HttpGet("Get/All/Suspects/{caseId}")]
        public async Task<IActionResult> GetAllSuspects(int caseId)
        {
            var result = await _caseService.GetParticipantsByRoleAsync(caseId, Role.Suspect);
            if (!result.Any()) return NotFound(new { message = "No suspects found for this case." });
            return Ok(result);
        }

        // get All Victims by ID
        [HttpGet("Get/All/Victims/{caseId}")]
        public async Task<IActionResult> GetAllVictims(int caseId)
        {
            var result = await _caseService.GetParticipantsByRoleAsync(caseId, Role.Victim);
            if (!result.Any()) return NotFound(new { message = "No victims found for this case." });
            return Ok(result);
        }

        // get All Witnesses by ID
        [HttpGet("Get/All/Witnesses/{caseId}")]
        public async Task<IActionResult> GetAllWitnesses(int caseId)
        {
            var result = await _caseService.GetParticipantsByRoleAsync(caseId, Role.Witness);
            if (!result.Any()) return NotFound(new { message = "No witnesses found for this case." });
            return Ok(result);
        }
    }
}

