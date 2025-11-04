using CrimeManagment.Models;

namespace CrimeManagment.DTOs
{
    public class CaseListDTO
    {
        public string CaseNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AreaCity { get; set; }
        public string CaseType { get; set; }
        public AuthorizationLevel AuthorizationLevel { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
