using Crime.Models;

namespace Crime.Repositories
{
    public interface ICaseAssigneesRepo : IGenericRepository<CaseAssignees>
    {
        Task<bool> AssignUserToCaseAsync(int caseId, int userId, AssigneeRole role);
        Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId);
        Task<int> GetCaseAuthorizationLevel(int caseId);
        Task<Users> GetUserByIdAsync(int id);
        Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus);
    }
}