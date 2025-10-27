using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crime.Models
{
    public enum EvidenceType
    {
        Text,
        Image
    }
    public class Evidence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvidenceId { get; set; }
        [Required]
        public EvidenceType Type { get; set; }
        public string? TextContent { get; set; }
        public string? FileUrl { get; set; }
        public string? MimeType { get; set; }
        public long SizeBytes { get; set; }
        public string? Remarks { get; set; }

        public bool IsSoftDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Case))]
        public int CaseId { get; set; } //FK to Cases
        public Cases Case { get; set; }

        [ForeignKey(nameof(AddedByUser))]
        public int AddedByUserId { get; set; } //FK to Users
        public Users AddedByUser { get; set; }
    }
}
