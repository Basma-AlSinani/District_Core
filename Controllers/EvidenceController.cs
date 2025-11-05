using Microsoft.AspNetCore.Mvc;
using CrimeManagment.Services;
using CrimeManagment.Models;
using static CrimeManagment.DTOs.EvidenceDTOS;
using CrimeManagment.DTOs;

namespace CrimeManagment.Controllers
{
    [Route("api/Evidence")]
    [ApiController]
    public class EvidenceController : ControllerBase
    {
        private readonly IEvidenceService _evidenceService;

        public EvidenceController(IEvidenceService evidenceService)
        {
            _evidenceService = evidenceService;
        }
        [HttpPost("CreateTextEvidence")]
        public async Task<IActionResult> CreateTextEvidence([FromBody] EvidenceDTOS.CreateTextEvidenceRequest request)
        {
            var result = await _evidenceService.CreateTextEvidenceAsync(request);
            return Ok(new { message = "Text evidence created successfully", evidence = result });
        }

        [HttpPost("CreateImageEvidence")]
        public async Task<IActionResult> CreateImageEvidence([FromForm] EvidenceDTOS.CreateImageEvidenceRequest request)
        {
            var result = await _evidenceService.CreateImageEvidenceAsync(request);
            return Ok(new { message = "Image evidence created successfully", evidence = result });
        }


        [HttpGet("GetAllEvidence")]
        public async Task<IActionResult> GetAllEvidence()
        {
            var evidences = await _evidenceService.GetAllAsync();
            return Ok(evidences);
        }

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetEvidenceById(int id)
        {
            var evidence = await _evidenceService.GetByIdAsync(id);
            if (evidence == null)
                return NotFound(new { message = "Evidence not found" });

            return Ok(evidence);
        }

       

        [HttpPut("UpdateTextEvidenceContent/{id}")]
        public async Task<IActionResult> UpdateTextEvidenceContent(int id, EvidenceUpdateTextRequest request)
        {
            try
            {
                await _evidenceService.UpdateContentAsync(id, request.TextContent, request.Remarks);
                return Ok(new { message = "Evidence content updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Image/{id}")]
        public async Task<IActionResult> GetEvidenceImage(int id)
        {
            try
            {
                var imageData = await _evidenceService.GetEvidenceImageAsync(id);
                return File(imageData, "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        [HttpGet("Retrieve/{id}")]
        public async Task<IActionResult> RetrieveEvidence(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found or is not soft deleted" });

                var response = new EvidenceResponse
                {
                    EvidenceId = evidence.EvidenceId,
                    CaseId = evidence.CaseId,
                    TextContent = evidence.TextContent,
                    FileUrl = evidence.FileUrl,
                    MimeType = evidence.MimeType,
                    SizeBytes = evidence.Type == EvidenceType.Image ? evidence.SizeBytes : null,
                    Remarks = evidence.Remarks,
                    Type = evidence.Type.ToString(),
                    CreatedAt = evidence.CreatedAt,
                    UpdatedAt = evidence.UpdatedAt
                };
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetImage/{id}")]
        public async Task<IActionResult> GetEvidenceImageById(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found" });

                if (evidence.Type != EvidenceType.Image)
                    return BadRequest(new { message = "Evidence is not an image" });

                var imageBytes = await _evidenceService.GetEvidenceImageAsync(id);

                if (imageBytes == null || imageBytes.Length == 0)
                    return NotFound(new { message = "Image file not found or empty." });

                return File(imageBytes, evidence.MimeType ?? "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateEvidence(int id, EvidenceDTOS.EvidenceUpdateTextRequest request)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found" });

                await _evidenceService.UpdateContentAsync(id, request.TextContent, request.Remarks);
                return Ok(new { message = "Evidence updated successfully (type not changed)" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteEvidence(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found" });

                await _evidenceService.SoftDeleteAsync(id);

                return Ok(new { message = "Evidence soft deleted successfully and audit log recorded." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDeleteEvidence(int id, [FromQuery] string? confirm = null, [FromQuery] string? command = null)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = $"Evidence with ID {id} not found." });

                if (confirm == null)
                    return BadRequest(new { message = $"Are you sure you want to permanently delete Evidence ID: {id}? (yes/no)" });

                if (confirm?.ToLower() != "yes")
                    return Ok(new { message = "Deletion canceled by user." });

                if (string.IsNullOrWhiteSpace(command) || command != $"DELETE {id}")
                    return BadRequest(new { message = $"To confirm deletion, send: DELETE {id}" });

                await _evidenceService.HardDeleteAsync(id);

                return Ok(new { message = $"Evidence ID {id} permanently deleted and logged for audit." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
