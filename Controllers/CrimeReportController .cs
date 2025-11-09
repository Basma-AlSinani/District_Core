using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CrimeManagementApi.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Services;
using System.Security.Claims;

namespace CrimeManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrimeReportController : ControllerBase
    {
        private readonly ICrimeReportsServie _crimeReportsService;

        public CrimeReportController(ICrimeReportsServie crimeReportsService)
        {
            _crimeReportsService = crimeReportsService;
        }

        // Public: Citizen report submission 
        [HttpPost("publicCreateReport")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateReport([FromBody] CreateCrimeReportsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var report = new CrimeReports
            {
                Title = dto.Title,
                Description = dto.Description,
                AreaCity = dto.AreaCity ?? string.Empty,
                Latitude = dto.Latitude ?? 0,
                Longitude = dto.Longitude ?? 0,
                CrimeStatus = CrimeStatus.Pending,
                ReportDataTime = DateTime.UtcNow,
                UserId = null
            };

            await _crimeReportsService.AddAsync(report);

            var resultDto = new CrimeReportDto
            {
                Id = report.CrimeReportId,
                Title = report.Title,
                Description = report.Description,
                AreaCity = report.AreaCity,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                ReportDateTime = report.ReportDataTime,
                Status = report.CrimeStatus.ToString()
            };

            return Ok(new { ReportId = resultDto.Id,
                            Message = "Report submitted successfully." });
        }

        // Authenticated: Logged-in officer/admin can file reports
        [Authorize]
        [HttpPost("CreateReport")]
        public async Task<IActionResult> CreateByUser([FromBody] CreateCrimeReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid user ID in token." });

            var report = new CrimeReports
            {
                Title = dto.Title,
                Description = dto.Description,
                AreaCity = dto.AreaCity ?? string.Empty,
                Latitude = dto.Latitude ?? 0,
                Longitude = dto.Longitude ?? 0,
                CrimeStatus = CrimeStatus.Pending,
                ReportDataTime = DateTime.UtcNow,
                UserId = dto.ReportedByUserId ?? 0
            };

            await _crimeReportsService.AddAsync(report);

            var resultDto = new CrimeReportDto
            {
                Id = report.CrimeReportId,
                Title = report.Title,
                Description = report.Description,
                AreaCity = report.AreaCity,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                ReportDateTime = report.ReportDataTime,
                Status = report.CrimeStatus.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        // Public: Check status by report ID
        [HttpGet("CheckstatusByReportId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus(int id)
        {
            var report = await _crimeReportsService.GetByIdAsync(id);
            if (report == null)
                return BadRequest(new { message = $"Report with ID {id} not found." });

            return Ok(new { ReportId = report.Id, Status = report.Status });
        }

            // Admin: List all reports
        [HttpGet("GetAllReport")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _crimeReportsService.GetAllAsync();
            var reportDtos = reports.Select(r => new CrimeReportDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                AreaCity = r.AreaCity,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                ReportDateTime = r.ReportDateTime,
                Status = r.Status
            });

            return Ok(reportDtos);
        }

        // Admin: View single report by ID
        [HttpGet("GetReportById{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = await _crimeReportsService.GetByIdAsync(id);
            if (report == null)
                return BadRequest(new { message = $"Report with ID {id} not found." });

            var resultDto = new CrimeReportDto
            {
                Id = report.Id,
                Title = report.Title,
                Description = report.Description,
                AreaCity = report.AreaCity,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                ReportDateTime = report.ReportDateTime,
                Status = report.Status
            };

            return Ok(resultDto);
        }
    }
}

