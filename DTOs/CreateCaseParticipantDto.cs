using System.ComponentModel.DataAnnotations;

namespace CrimeManagment.DTOs
{
    // DTO for case participant details
    public class CaseParticipantDto
    {
        public int CaseParticipantId { get; set; }
        public int CaseId { get; set; }
        public int ParticipantId { get; set; }
        public DateTime LinkedAt { get; set; }
        public string? Notes { get; set; }
    }

    //  linking participants to cases
    public class CreateCaseParticipantDto
    {     
        public int CaseId { get; set; }
        public int ParticipantId { get; set; }
        public int? AddedByUserId { get; set; }
        public string? Notes { get; set; }
    }

    // editing case link details
    public class UpdateCaseParticipantDto
    {
        public int? CaseId { get; set; }
        public int? ParticipantId { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

