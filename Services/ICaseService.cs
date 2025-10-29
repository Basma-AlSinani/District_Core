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
    }
}