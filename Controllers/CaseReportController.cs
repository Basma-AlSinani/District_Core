using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CrimeManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CaseReportController : ControllerBase
    {
        private readonly ICaseReportService _service;

        public CaseReportController(ICaseReportService service)
        {
            _service = service;
        }

        // Returns all case-report links
        [HttpGet("GetAllCaseReportLinks")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Returns a single case-report by its unique ID
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null)
                    return NotFound("CaseReport not found");

                return Ok(entity);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Creates a new case-report link
        [HttpPost("CreatesNewCaseReportLink")]
        public async Task<IActionResult> Create([FromBody] CreateCaseReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var entity = new CaseReports
                {
                    CaseId = dto.CaseId,
                    CrimeReportId = dto.CaseReportId,
                    Notes = dto.Notes,
                    LinkedAt = DateTime.UtcNow
                };

                await _service.AddAsync(entity);
                return Ok(entity);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Updates an existing case-report by its ID
        [HttpPut("UpdatesCaseReportByID/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCaseReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _service.GetByIdAsync(id);
                if (existing == null)
                    return NotFound("CaseReport not found");

                if (dto.CaseId.HasValue)
                    existing.CaseId = dto.CaseId.Value;

                if (dto.CaseReportId.HasValue)
                    existing.CrimeReportId = dto.CaseReportId.Value;

                if (!string.IsNullOrEmpty(dto.Notes))
                    existing.Notes = dto.Notes;

                existing.LinkedAt = dto.UpdatedAt ?? DateTime.UtcNow;

                var result = await _service.UpdateAsync(existing);
                if (!result)
                    return BadRequest("Update failed");

                return Ok(existing);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Deletes a case-report by its ID
        [HttpDelete("DeletesCaseReportByID/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success)
                    return NotFound("CaseReport not found");

                return Ok("Deleted successfully");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
