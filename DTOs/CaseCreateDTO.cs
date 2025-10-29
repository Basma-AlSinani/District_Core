using Crime.Models;
using System.ComponentModel.DataAnnotations;

namespace Crime.DTOs
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
}
