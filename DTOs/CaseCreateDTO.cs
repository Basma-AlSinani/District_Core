using CrimeManagment.Models;
using System.ComponentModel.DataAnnotations;

namespace CrimeManagment.DTOs
{
    // DTO for creating a new case
    public class CaseCreateDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AreaCity { get; set; }

        public string CaseType { get; set; }

        public Status Status { get; set; } 

        public AuthorizationLevel AuthorizationLevel { get; set; }

        public int CreatedByUserId { get; set; }

        public List<int> CrimeReportIds { get; set; } = new();
    }

    public class UpdateCaseDTO
    {
        public string? Description { get; set; }

        public ProgreessStatus? Status { get; set; }

        public int? AssignedToUserId { get; set; }

        public List<int>? AddCrimeReportId { get; set; }
        public CrimeStatus? CrimeStatus { get; set; }
    }
}
