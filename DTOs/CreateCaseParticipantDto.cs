using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CrimeManagment.DTOs
{
    public class ParticipantDto
    {
        public int ParticipantsId { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int CaseId { get; set; }

        public int? AddedByUserId { get; set; }

        public DateTime AddedOn { get; set; }
    }

    public class CreateParticipantDto
    {
        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [Required]
        public int CaseId { get; set; }

        [AllowNull]
        public int? AddedByUserId { get; set; }
    }


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

