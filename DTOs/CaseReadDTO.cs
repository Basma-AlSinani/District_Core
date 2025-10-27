using Crime.Models;

namespace Crime.DTOs
{
    // DTO for reading case details
    public class CaseReadDTO
    {
        public int CaseId { get; set; }
        public string CaseNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AreaCity { get; set; }
        public string CaseType { get; set; }
        public AuthorizationLevel AuthorizationLevel { get; set; }
        public Status Status { get; set; }
        public string CreatedByUserFullName { get; set; } // Full name of the user who created the case
        public DateTime CreatedAt { get; set; }
    }
}
