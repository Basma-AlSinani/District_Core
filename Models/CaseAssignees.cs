using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public enum AssigneeRole
    {
        Investigator,
        Officer
    }

    public enum ProgressStatus
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
        public string AssigneeRole { get; set; }
        public string ProgreessStatus { get; set; }
        public DateTime AssignedAt { get; set; }

        [ForeignKey("Cases")]
        public int CaseId { get; set; }
        public Cases Cases { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }
    }
}
