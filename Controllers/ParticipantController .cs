using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CrimeManagment.DTOs;
using CrimeManagment.Services;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
    private readonly ParticipantService _participantService;
    private readonly ILogger<ParticipantController> _logger;

    public ParticipantController(ParticipantService participantService, ILogger<ParticipantController> logger)
    {
        _participantService = participantService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("Add/Participant")]
    public async Task<IActionResult> Create([FromBody] CreateParticipantDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim))
                dto.AddedByUserId = int.Parse(userIdClaim);

            var participant = await _participantService.CreateAsync(dto);

            if (participant == null)
                return StatusCode(500, new { message = "Failed to create participant." });

            return CreatedAtAction(nameof(GetById), new { id = participant.ParticipantsId }, participant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding participant");
            return StatusCode(500, new { message = "An error occurred while adding the participant." });
        }
    }

 
    [Authorize]
    [HttpGet("Get/By/Id{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var participant = await _participantService.GetByIdAsync(id);
        if (participant == null)
            return NotFound();

        return Ok(participant);
    }


    [Authorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var participants = await _participantService.GetAllAsync();
        return Ok(participants);
    }
}
