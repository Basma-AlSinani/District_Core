namespace Crime.Models
{
    public class Cases
    {
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AreaCity { get; set; }
        public string CaseType { get; set; }
        public string AuthorizationLevel { get; set; }
        public string Status { get; set; }

        public int CreatedByUserId { get; set; } //FK to Users
        public Users CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
