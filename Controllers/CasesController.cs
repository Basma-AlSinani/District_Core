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
        private readonly ICaseService _casesService;

        public CasesController(ICaseService casesService)
        {
            _casesService = casesService;
        }

        [HttpPost("submit-report")]
        public async Task<IActionResult> SubmitCrimeReport([FromBody] CrimeReportCreateDTO dto)
        {
            var report = await _casesService.SubmitCrimeReportAsync(dto);
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

            var newCase = await _casesService.CreateCaseAsync(dto);

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
            var updatedCase = await _casesService.UpdateCaseAsync(id, dto);
            return Ok(new
            {
                Message = "Case updated successfully",
                CaseId = updatedCase.CaseId
            });
        }
    }
}
