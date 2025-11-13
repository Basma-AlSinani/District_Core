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
        private readonly ICrimeReportsService _crimeReportsService;
        private readonly ILogger<CrimeReportController> _logger;

        public CrimeReportController(ICrimeReportsService crimeReportsService, ILogger<CrimeReportController> logger)
        {
            _crimeReportsService = crimeReportsService;
            _logger = logger;
        }
        //For public users to create crime reports
        [HttpPost("public/submit-report")]
        [AllowAnonymous]
        public async Task<IActionResult> PublicCreateReport([FromBody] CreateCrimeReportDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            var reportDto = await _crimeReportsService.CreateReportAsync(dto);

            return Ok(new
            {
                ReportId = reportDto.Id,
                Message = "Report submitted successfully. An email notification has been sent."
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
        [Authorize] 
        public async Task<IActionResult> CreateReportByUser([FromBody] CreateCrimeReportByUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized(new { Message = "Invalid user context." });

                dto.ReportedByUserId = userId;

                
                var reportDto = await _crimeReportsService.CreateReportAsync(
                    new CreateCrimeReportDto
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        AreaCity = dto.AreaCity,
                        Latitude = dto.Latitude,
                        Longitude = dto.Longitude,
        
                    },
                    dto.ReportedByUserId
                );

                return Ok(new
                {
                    ReportId = reportDto.Id,
                    Message = "Report submitted successfully. An email notification has been sent."
                });
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