using Crime.Models;
using System.ComponentModel.DataAnnotations;

namespace Crime.DTOs
{
    // DTO for creating a case participant
    public class CaseParticipantCreateDTO
    {
        [Required]
        public int CaseId { get; set; }

        [Required]
        public int ParticipantId { get; set; }

        [Required]
        public Role Role { get; set; }

        public int? AddedByUserId { get; set; }
    }
}
