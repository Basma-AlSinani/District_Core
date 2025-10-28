namespace Crime.DTOs
{
    public class BulkLinkRequestDto
    {
        public int CaseId { get; set; }
        public List<int> CrimeReportId { get; set; }
        public int? PerformedBy { get; set; }
    }
}
