using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CrimeManagment.Services;
using CrimeManagment.DTOs;
using CrimeManagement.DTOs;


namespace CrimeManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthDtos.LoginDtos dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _authService.LoginAsync(dto);
            if (response == null)
            {
                _logger.LogWarning("Failed login for {Email}", dto.Email);
                return Unauthorized("Invalid username or password.");
            }

            _logger.LogInformation("User {Email} logged in.", response.Email);
            return Ok(response);

        }

    }
}
