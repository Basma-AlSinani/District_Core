using CrimeManagment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrimeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(string to, string subject, string body)
        {
            await _emailService.SendEmailAsync(to, subject, body);
            return Ok(new { Message = $"Email sent to {to}" });
        }

        [HttpPost("SendCaseUpdate")]
        public async Task<IActionResult> SendCaseUpdate(string to, string caseId, string updateDetails)
        {
            await _emailService.SendCaseUpdateAsync(to, caseId, updateDetails);
            return Ok(new { Message = $"Case update email sent to {to} for case {caseId}" });
        }

        [HttpPost("SendNewCrimeAlert")]
        public async Task<IActionResult> SendNewCrimeAlert(string to, string crimeDetails)
        {
            await _emailService.SendNewCrimeAlertAsync(to, crimeDetails);
            return Ok(new { Message = $"New crime alert email sent to {to}" });
        }
    }
}
