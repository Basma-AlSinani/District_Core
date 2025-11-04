using System.ComponentModel.DataAnnotations;

namespace CrimeManagment.DTOs
{
    public class CaseReportDto
    {
        // Unique identifier for this link
        public int Id { get; set; }

        [Required]
        public int CaseId { get; set; }
 
        [MaxLength(200)]
        public string? CaseName { get; set; }

        [Required]
        public int CaseReportId { get; set; }

        [MaxLength(150)]
        public string? ReportTitle { get; set; }

        [MaxLength(100)]
        public string? AreaCity { get; set; } // City or area of the incident.

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   
        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class CreateCaseReportDto
    {
        [Required]
        public int CaseId { get; set; }
        public int CaseReportId { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdateCaseReportDto
    {
        public int? CaseId { get; set; }
        public int? CaseReportId { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
