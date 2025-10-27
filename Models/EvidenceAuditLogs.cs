using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public enum EvidenceAction
    {
        Add,
        Updated,
        SoftDeleted,
        HardDeleted,
    }
    public class EvidenceAuditLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvidenceAuditLogId { get; set; }

        [Required, MaxLength(50)]
        public string Action { get; set; }
        public DateTime ActedAt { get; set; }
        [MaxLength(1000)]
        public string Details { get; set; }

        [ForeignKey("Evidence")]
        public int EvidenceItemId { get; set; }
        public Evidence Evidence { get; set; }
    }
}
