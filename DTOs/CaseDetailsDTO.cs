namespace Crime.DTOs
{
    public class CaseDetailsDTO
    {
        public string CaseNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AreaCity { get; set; }
        public string CaseType { get; set; }
        public string CaseLevel { get; set; }  
        public string AuthorizationLevel { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ReportedBy { get; set; } // User who reported the case
        public int NumberOfAssignees { get; set; }
        public int NumberOfEvidences { get; set; }
        public int NumberOfSuspects { get; set; }
        public int NumberOfVictims { get; set; }
        public int NumberOfWitnesses { get; set; }
    }
}
