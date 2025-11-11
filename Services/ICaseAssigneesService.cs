using CrimeManagment.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface ICaseAssigneesService
    {
        Task AddAsync(CaseAssignees entity);
        Task<(bool Success, string Message)> AssignUserToCaseAsync(int caseId, int assignerId, int assigneeId, AssigneeRole role);
        Task DeleteAsync(int id);
        Task<IEnumerable<CaseAssignees>> GetAllAsync();
        Task<IEnumerable<CaseAssigneesDTOs.CaseAssigneeDTOs>> GetAssigneesByCaseIdAsync(int caseId);
        Task<CaseAssignees> GetByIdAsync(int id);
        Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus);
        Task UpdateAsync(CaseAssignees entity);
    }
}