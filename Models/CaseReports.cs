using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public class CaseReports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseReportId { get; set; }

        public DateTime LinkedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Cases")]
        public int CaseId { get; set; }
        public Cases cases { get; set; }

        [ForeignKey("CrimeReports")]
        public int CrimeReportId { get; set; }
        public CrimeReports CrimeReports { get; set; }

        [ForeignKey("Users")]
        public int? PerformedBy { get; set; } //to regester who performed the action (linking)
        public Users? Users { get; set; } // navigation property to Users

        [MaxLength(500)]
        public string? Notes { get; set; }

    }
}
