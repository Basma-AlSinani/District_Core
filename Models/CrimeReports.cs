using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public enum CrimeStatus
    {
        pending,
        en_route,
        on_scene,
        under_investigation,
        resolved,
        reported
    }
    public class CrimeReports
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CrimeReportId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public string AreaCity { get; set; }
        public DateTime ReportDataTime { get; set; }
        [Required]
        public CrimeStatus CrimeStatus { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users Users { get; set; }

    }
}
