using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public class CaseReports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseReportId { get; set; }
        public DateTime LinkedAt { get; set; }
        [ForeignKey("Cases")]
        public int CaseId { get; set; }
        public Cases Cases { get; set; }
        [ForeignKey("CrimeReports")]
        public int CrimeReportId { get; set; }
        public CrimeReports CrimeReports { get; set; }
    }
}
