namespace Crime.Models
{
    public class CaseParticipants
    {
        [Key]
        public int CaseParticipantId { get; set; }

        public int CaseId { get; set; } //FK to Cases
        public Cases Case { get; set; }

        public int ParticipantId { get; set; } //FK to Participants
        public Participants Participant { get; set; }

        public string Role { get; set; }
        public string AddedByUserId { get; set; } //FK to Users
        public Users AddedByUser { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
