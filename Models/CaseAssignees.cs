using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public enum AssigneeRole
    {
        Investigator,
        Officer
    }

    public enum ProgreessStatus
    {
        Pending,
        InProgress,
        Completed,
        Closed
    }
    public class CaseAssignees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseAssigneeId { get; set; }

        [Required]
        public AssigneeRole AssigneeRole { get; set; }

        [Required]
        public ProgreessStatus ProgreessStatus { get; set; }

        public DateTime AssignedAt { get; set; }= DateTime.UtcNow;

        [ForeignKey("Cases")]
        public int CaseId { get; set; }
        public Cases Cases { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }

        [ForeignKey("AssignedBy")]
        public int AssignedByUserId { get; set; }
        public Users AssignedBy { get; set; }
    }
}
