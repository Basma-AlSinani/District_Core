using Crime.DTOs;
using Crime.Repositories;
using Crime.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
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

        [HttpPost("submit-report")]
        public async Task<IActionResult> SubmitCrimeReport([FromBody] CrimeReportCreateDTO dto)
        {
            var report = await _caseService.SubmitCrimeReportAsync(dto);
            return Ok(new
            {
                Message = "Crime report submitted successfully",
                CrimeReportId = report.CrimeReportId
            });
        }

        [HttpPost] 
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

        [HttpPut("{id}")]
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
        [HttpGet("list")]
        public async Task<IActionResult> GetCases([FromQuery] string? search)
        {
            var cases = await _caseService.GetCasesAsync(search);
            return Ok(cases);
        }

        // Get case details by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaseDetails(int id)
        {
            try
            {
                var caseDetails = await _caseService.GetCaseDetailsAsync(id);
                return Ok(caseDetails);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
