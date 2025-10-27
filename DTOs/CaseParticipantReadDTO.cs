using Crime.Models;

namespace Crime.DTOs
{
    // DTO for reading case participant details
    public class CaseParticipantReadDTO
    {
        public int CaseParticipantId { get; set; }
        public string ParticipantFullName { get; set; }
        public Role Role { get; set; }
        public string AddedByUserFullName { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
