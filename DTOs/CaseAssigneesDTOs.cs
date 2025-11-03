using Crime.Models;

namespace Crime.DTOs
{
    public class CaseAssigneesDTOs
    {
        public class AssignUserDTO
        {
            public int CaseId { get; set; }
            public int AssignerId { get; set; }  // The user performing the assignment
            public int AssigneeId { get; set; }  // The user being assigned
            public AssigneeRole Role { get; set; }
        }

        public class UpdateAssigneeStatusDTO
        {
            public ProgreessStatus NewStatus { get; set; }
        }
    }
}
