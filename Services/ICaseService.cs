using Crime.DTOs;
using Crime.Models;

namespace Crime.Services
{
    public interface ICaseService
    {
        Task<Cases> CreateCaseAsync(CaseCreateDTO dto);
        Task<IEnumerable<CaseListDTO>> GetCasesAsync(string? search = null);
        Task<CrimeReports> SubmitCrimeReportAsync(CrimeReportCreateDTO dto);
        Task<Cases> UpdateCaseAsync(int caseId, UpdateCaseDTO dto);
        Task<CaseDetailsDTO> GetCaseDetailsAsync(int caseId);
        Task<IEnumerable<object>> GetAssigneesByCaseIdAsync(int caseId);
        Task<IEnumerable<object>> GetEvidenceByCaseIdAsync(int caseId);
        Task<IEnumerable<object>> GetParticipantsByRoleAsync(int caseId, Role role);
    }
}
