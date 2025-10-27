namespace District_Core.Models
{
    public class Evidence
    {
        public int EvidenceId { get; set; }
        public int CaseId { get; set; } //FK to Cases
        public int AddedByUserId { get; set; } //FK to Users
        public string Type { get; set; }
        public string TextContent { get; set; }
        public string FileUrl { get; set; }
        public string MimeType { get; set; }
        public int SizeBytes { get; set; }
        public string Remarks { get; set; }
        public bool IsSoftDeleted { get; set; }
        public DateTime CreatrdAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
