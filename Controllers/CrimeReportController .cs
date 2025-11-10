using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CrimeManagementApi.DTOs;
using CrimeManagment.Services;
using System.Security.Claims;

namespace CrimeManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrimeReportController : ControllerBase
    {
        private readonly ICrimeReportsServie _crimeReportsService;
        private readonly ILogger<CrimeReportController> _logger;

        public CrimeReportController(ICrimeReportsServie crimeReportsService, ILogger<CrimeReportController> logger)
        {
            _crimeReportsService = crimeReportsService;
            _logger = logger;
        }
        //For public users to create crime reports
        [HttpPost("public/submit-report")]
        [AllowAnonymous]
        public async Task<IActionResult> PublicCreateReport([FromBody] CreateCrimeReportsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var report = new CrimeManagment.Models.CrimeReports
            {
                Title = dto.Title,
                Description = dto.Description,
                AreaCity = dto.AreaCity ?? string.Empty,
                Latitude = dto.Latitude ?? 0,
                Longitude = dto.Longitude ?? 0,
                CrimeStatus = CrimeManagment.Models.CrimeStatus.Pending,
                ReportDataTime = DateTime.UtcNow,
                UserId = null
            };

            await _crimeReportsService.AddAsync(report);

            return Ok(new
            {
                ReportId = report.CrimeReportId,
                Message = "Report submitted successfully."
            });
        }

        [HttpGet("status/{id}public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus(int id)
        {
            var status = await _crimeReportsService.GetStatusAsync(id);
            return Ok(new { ReportId = id, Status = status });
        }



        [HttpPost("user/submit-report")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateByUser([FromBody] CreateCrimeReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { Message = "You are not authorized to submit a report." });

                dto.ReportedByUserId = int.Parse(userIdClaim);


                var reportDto = await _crimeReportsService.CreateReportAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = reportDto.Id }, reportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating report with user context.");
                return StatusCode(500, new { message = "Error creating report with user context." });
            }
        }
        [HttpGet("Get/All/Crime/Report")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { Message = "You are not authorized to view reports." });

            var reports = await _crimeReportsService.GetAllAsync();
            return Ok(reports);
        }

        [HttpGet("Get/Crime/Report/By{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new { Message = "You are not authorized to view reports." });

            var report = await _crimeReportsService.GetByIdAsync(id);
            return report == null ? NotFound(new { Message = $"Report with ID {id} not found." }) : Ok(report);
        }
    }
}