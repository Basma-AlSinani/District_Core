using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICasesRepo : IGenericRepository<Cases>
    {
        Task<IEnumerable<Cases>> GetAllAsync();
        Task<IEnumerable<Users>> GetAssigneesByCaseIdAsync(int caseId);
        Task<Cases> GetCaseByNumberAsync(string caseNumber);
        Task<Cases> GetCaseDetailsByIdAsync(int id);
        Task<IEnumerable<Evidence>> GetEvidenceByCaseIdAsync(int caseId);
        Task<IEnumerable<CaseParticipants>> GetParticipantsByRoleAsync(int caseId, Role role);
    }
}