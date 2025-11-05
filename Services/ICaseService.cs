using CrimeManagment.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface ICaseService
    {
        Task<Cases> CreateCaseAsync(CaseCreateDTO dto);
        Task<bool> DeleteCaseAsync(int caseId);
        Task<IEnumerable<object>> GetAssigneesByCaseIdAsync(int caseId);
        Task<CaseDetailsDTO> GetCaseDetailsAsync(int id);
        Task<IEnumerable<CaseListDTO>> GetCasesAsync();
        Task<IEnumerable<object>> GetEvidenceByCaseIdAsync(int caseId);
        Task<IEnumerable<object>> GetParticipantsByRoleAsync(int caseId, Role role);
        Task<Cases> UpdateCaseAsync(int caseId, UpdateCaseDTO dto);
    }
}