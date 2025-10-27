using Crime.Models;
using System.ComponentModel.DataAnnotations;

namespace Crime.DTOs
{
    // DTO for creating a new case
    public class CaseCreateDTO
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string AreaCity { get; set; }

        [Required]
        public string CaseType { get; set; }

        [Required]
        public AuthorizationLevel AuthorizationLevel { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}
