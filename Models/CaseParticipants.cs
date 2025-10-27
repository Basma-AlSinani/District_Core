using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public enum Role
    {
        Suspect,
        Victim,
        Witness
    }

    public class CaseParticipants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseParticipantId { get; set; }

        [ForeignKey(nameof(Case))]
        public int CaseId { get; set; } //FK to Cases
        public Cases Case { get; set; }

        [ForeignKey(nameof(Participants))]
        public int ParticipantId { get; set; } //FK to Participants
        public Participants Participant { get; set; }

        [Required]
        public Role Role { get; set; }

        [ForeignKey(nameof(Users))]
        public int? AddedByUserId { get; set; } //FK to Users
        public Users AddedByUser { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
