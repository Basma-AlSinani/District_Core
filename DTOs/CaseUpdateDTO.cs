using Crime.Models;

namespace Crime.DTOs
{
    public class CaseUpdateDTO
    {
        public string? Description { get; set; }
        public Status? Status { get; set; }
        public List<int>? AddCrimeReportId { get; set; }
    }
}
