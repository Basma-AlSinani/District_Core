using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace District_Core.Models
{
    public enum CrimeStatus
    {
        Reported,
        UnderInvestigation,
        Resolved,
        Closed
    }
    public class CrimeReports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CrimeReportId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string AreaCity { get; set; }
        public DateTime ReportDataTime { get; set; }
        public string CrimeStatus { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }

    }
}
