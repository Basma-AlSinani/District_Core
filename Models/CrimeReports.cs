using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public enum CrimeStatus
    {
        Pending,
        En_route,
        On_scene,
        Under_investigation,
        Resolved,
        Reported
    }
    public class CrimeReports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CrimeReportId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, MaxLength(500)]
        public string Description { get; set; }
        [Required]
        public string AreaCity { get; set; }
        public DateTime ReportDataTime { get; set; } = DateTime.UtcNow;
        [Required]
        public CrimeStatus CrimeStatus { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }
        // Navigation property for related CaseReports
        public ICollection<CaseReports> CaseReports { get; set; } = new List<CaseReports>();
    }
}
