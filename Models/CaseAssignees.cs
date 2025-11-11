using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public enum AssigneeRole
    {
        Officer = 1,
        Investigator = 2,
        Admin = 3
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
        public AssigneeRole Role { get; set; }

        [Required]
        public ProgreessStatus Status { get; set; }

        public DateTime AssignedAt { get; set; }= DateTime.UtcNow;

        [ForeignKey("Cases")]
        public int CaseId { get; set; }
        public Cases Cases { get; set; }

        [ForeignKey("AssignedTo")]
        public int AssignedToUserId { get; set; }
        public Users AssignedTo { get; set; }

        [ForeignKey("AssignedBy")]
        public int AssignedByUserId { get; set; }
        public Users AssignedBy { get; set; }
    }
}
