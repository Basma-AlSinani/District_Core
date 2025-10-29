using Crime.Models;

namespace Crime.Services
{
    public interface ICaseReportService
    {
        Task AddAsync(CaseReports entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<CaseReports>> GetAllAsync();
        Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId);
        Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId);
        Task<CaseReports> GetByIdAsync(int id);
        Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId);
        Task UpdateAsync(CaseReports entity);
    }
}