using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrimeManagment.Models
{
    public enum AuthorizationLevel
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum Status
    {
        Pending,
        Ongoing,
        Closed,
    }
    public class Cases
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseId { get; set; }

        [Required, MaxLength(50)]
        public string CaseNumber { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string AreaCity { get; set; }

        [MaxLength(50)]
        public string CaseType { get; set; }

        [Required]
        public AuthorizationLevel AuthorizationLevel { get; set; }

        [Required]
        public Status Status { get; set; }

        [ForeignKey(nameof(Users))]
        public int CreatedByUserId { get; set; } //FK to Users
        public Users CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<CaseReports> CaseReports { get; set; }
        public ICollection<CaseComment> CaseComments { get; set; } = new List<CaseComment>();

    }
}
