namespace Crime.DTOs
{
    public class LinkRequestDto
    {
        public int CaseId { get; set; }
        public int CrimeReportId { get; set; }
        public int? PerformedBy { get; set; }
    }
}
