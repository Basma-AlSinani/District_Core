using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICaseReportRepo
    {
        Task AddAsync(CaseReports entity);
        Task DeleteAsync(CaseReports entity);
        Task<bool> ExistsAsync(int caseId, int crimeReportId);
        Task<IEnumerable<CaseReports>> GetAllWithRelationsAsync();
        Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId);
        Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId);
        Task<CaseReports> GetByIdAsync(int id);
        Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId);
        Task<CaseReports?> GetDetailsAsync(int id);
        Task<IEnumerable<CaseReports>> SearchCasesAsync(string? keyword);
        Task UpdateAsync(CaseReports entity);
    }
}