using Microsoft.AspNetCore.Mvc;
using CrimeManagment.Services;
using CrimeManagment.Models;
using static CrimeManagment.DTOs.EvidenceDTOS;
using CrimeManagment.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CrimeManagment.Controllers
{
    [Authorize(Roles = "Admin,Investigator,Officer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EvidenceController : ControllerBase
    {
        private readonly IEvidenceService _evidenceService;

        public EvidenceController(IEvidenceService evidenceService)
        {
            _evidenceService = evidenceService;
        }

        // Helper: Map Evidence to Response
        private EvidenceResponse MapToResponse(Evidence evidence)
        {
            return new EvidenceResponse
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
        }

        // Create Text Evidence
        [HttpPost("CreateTextEvidence")]
        public async Task<IActionResult> CreateTextEvidence([FromBody] CreateTextEvidenceRequest request)
        {
            try
            {
                var evidence = await _evidenceService.CreateTextEvidenceAsync(request);
                return Ok(new { message = "Text evidence created successfully", evidence = MapToResponse(evidence) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Create Image Evidence
        [HttpPost("CreateImageEvidence")]
        public async Task<IActionResult> CreateImageEvidence([FromForm] CreateImageEvidenceRequest request)
        {
            try
            {
                var evidence = await _evidenceService.CreateImageEvidenceAsync(request);
                return Ok(new { message = "Image evidence created successfully", evidence = MapToResponse(evidence) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get All Evidence
        [HttpGet("GetAllEvidence")]
        public async Task<IActionResult> GetAllEvidence()
        {
            var evidences = await _evidenceService.GetAllAsync();
            var response = evidences.Select(MapToResponse);
            return Ok(response);
        }

        // Get Evidence by ID
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetEvidenceById(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found" });

                return Ok(MapToResponse(evidence));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Update Text Content
        [Authorize(Roles = "Admin,Investigator")]
        [HttpPut("UpdateTextEvidenceContent/{id}")]
        public async Task<IActionResult> UpdateTextEvidenceContent(int id, EvidenceUpdateTextRequest request)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found" });

                await _evidenceService.UpdateContentAsync(id, request.TextContent, request.Remarks);
                return Ok(new { message = "Evidence content updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get Image File
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

        // Soft Delete
        [Authorize(Roles = "Admin,Investigator")]
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

        // Hard Delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDeleteEvidence(int id, [FromBody] DeleteEvidenceRequest request)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = $"Evidence with ID {id} not found." });

                if (request.Confirm.ToLower() != "yes")
                    return Ok(new { message = "Deletion canceled by user." });

                await _evidenceService.HardDeleteAsync(id);
                return Ok(new { message = $"Evidence ID {id} permanently deleted and logged for audit." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Retrieve Soft Deleted Evidence
        [Authorize(Roles = "Admin,Investigator")]
        [HttpGet("Retrieve/{id}")]
        public async Task<IActionResult> RetrieveEvidence(int id)
        {
            try
            {
                var evidence = await _evidenceService.GetByIdAsync(id);
                if (evidence == null)
                    return NotFound(new { message = "Evidence not found or is not soft deleted" });

                return Ok(MapToResponse(evidence));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
