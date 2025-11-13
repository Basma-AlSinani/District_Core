using CrimeManagment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static CrimeManagment.DTOs.EmailDTOs;

namespace CrimeManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ResidentsApiClient _residentsApiClient;
        public EmailController(IEmailService emailService, ResidentsApiClient residentsApiClient)
        {
            _emailService = emailService;
            _residentsApiClient = residentsApiClient;
        }
        private bool IsValidEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                return false;
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                if (!IsValidEmail(request.To))
                    return BadRequest(new { Message = "Invalid email address." });
                
                await _emailService.SendEmailAsync(request.To,request.Subject,request.Body);
                return Ok(new { Message = $"Email sent to {request.To}" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = $"Failed to send email: {ex.Message}" });
            }
        }

        [HttpPost("SendCaseUpdate")]
        public async Task<IActionResult> SendCaseUpdate([FromBody] CaseUpdateRequest request)
        {
            try
            {
                if (!IsValidEmail(request.To))
                    return BadRequest(new { Message = "Invalid email address." });

                await _emailService.SendCaseUpdateAsync(request.To, request.CaseId, request.UpdateDetails);
                return Ok(new { Message = $"Case update email sent to {request.To} for case {request.CaseId}" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = $"Failed to send case update email: {ex.Message}" });
            }
            
        }

        [HttpPost("SendNewCrimeAlert")]
        public async Task<IActionResult> SendNewCrimeAlert([FromBody] CrimeAlertRequest request)
        {
            try
            {
                if (!IsValidEmail(request.To))
                    return BadRequest(new { Message = "Invalid email address." });
                await _emailService.SendNewCrimeAlertAsync(request.To, request.CrimeDetails);
                return Ok(new { Message = $"New crime alert email sent to {request.To}" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = $"Failed to send new crime alert email: {ex.Message}" });
            }
        }

        
        [HttpPost("SendResidentsEmails")]
        public async Task<IActionResult> SendResidentsEmails()
        {
            var residents = await _residentsApiClient.GetAllResidentsAsync();
            foreach (var resident in residents)
            {
                await _emailService.SendEmailAsync(
                    resident.Email,
                    "Verification Notice",
                    $"Dear {resident.FullName}, please verify your registration."
                );
            }
            return Ok(new { Message = $"Emails sent to {residents.Count()} residents." });
        }
    }
}
