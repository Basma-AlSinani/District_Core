using Crime.Models;

namespace Crime.Repositories
{
    public interface ICaseAssigneesRepo : IGenericRepository<CaseAssignees>
    {
        Task<bool> AssignUserToCaseAsync(int caseId, int userId, AssigneeRole role);
        Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId);
        Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus);
    }
}