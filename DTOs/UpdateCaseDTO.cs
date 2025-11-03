using CrimeManagment.Models;

namespace CrimeManagment.DTOs
{
    public class UpdateCaseDTO
    {
        public string? Description { get; set; }

        public ProgreessStatus? Status { get; set; }

        public int? AssignedToUserId { get; set; }

        public AssigneeRole? Role { get; set; }
        public List<int>? AddCrimeReportId { get; set; }
    }
}
