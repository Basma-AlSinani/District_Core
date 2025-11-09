using CrimeManagment.DTOs;
using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface ICaseService
    {
        Task<Cases> CreateCaseAsync(CaseCreateDTO dto);
        Task<bool> DeleteCaseAsync(int caseId);
        Task<CaseDetailsDTO> GetCaseDetailsAsync(int id);
        Task<IEnumerable<CaseListDTO>> GetCasesAsync();
        Task<(Cases? updatedCase, string? error)> UpdateCaseAsync(int caseId, UpdateCaseDTO dto);
    }
}