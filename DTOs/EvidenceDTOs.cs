using CrimeManagment.Models;

namespace CrimeManagment.DTOs
{
    public class EvidenceDTOS
    {
        public class EvidenceCreateRequest
        {
            public int CaseId { get; set; }
            public EvidenceType Type { get; set; }
            public string? TextContent { get; set; } //text content for Text type
            public IFormFile? File { get; set; } //file for Image type
            public string? Remarks { get; set; }

            public Users CreatedByUser { get; set; } //user who is adding the evidence
        }

        public class EvidenceUpdateTextRequest
        {
            public string? TextContent { get; set; }
            public string? Remarks { get; set; }
        }

        public class EvidenceResponse
        {
            public int EvidenceId { get; set; }
            public int CaseId { get; set; }
            public string? TextContent { get; set; }
            public string? FileUrl { get; set; }
            public string? MimeType { get; set; }
            public long? SizeBytes { get; set; }
            public string Type { get; set; }
            public string? Remarks { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            
        }
    }
}
