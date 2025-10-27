namespace District_Core.Models
{
    public class CaseParticipants
    {
        public int CaseParticipantId { get; set; }
        public int CaseId { get; set; } //FK to Cases
        public int ParticipantId { get; set; }
        public string Role { get; set; }
        public string AddedByUserId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
