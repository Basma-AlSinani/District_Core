namespace District_Core.Models
{
    public class EvidenceAuditLogs
    {
        public int EvidenceAuditLogId { get; set; }
        public string Action { get; set; }
        public DateTime ActedAt { get; set; }
        public string Details { get; set; }
    }
}
