using CrimeManagment.Models;

namespace CrimeManagment.DTOs
{
    public class CaseAssigneesDTOs
    {
        public class AssignUserDTO
        {
            public int CaseId { get; set; }
            //public int AssignerId { get; set; }  // The user performing the assignment
            public int AssigneeId { get; set; }  // The user being assigned
            //public AssigneeRole Role { get; set; }
        }

        public class UpdateAssigneeStatusDTO
        {
            public ProgreessStatus NewStatus { get; set; }
        }
        public class CaseAssigneeDTOs
        {
            public int CaseAssigneeId { get; set; }
            public int CaseId { get; set; }
            public string Role { get; set; }
            public string Status { get; set; }
            public DateTime AssignedAt { get; set; }

            public string AssignedToName { get; set; }
            public string AssignedToRole { get; set; }

            public string AssignedByName { get; set; }

            public string CaseNumber { get; set; }
            public string CaseName { get; set; }
        }

    }
}
