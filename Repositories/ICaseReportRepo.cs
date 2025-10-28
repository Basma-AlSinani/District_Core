using Crime.Models;

namespace Crime.Repositories
{
    public interface ICaseReportRepo : IGenericRepository<CaseReports>
    {
        Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId);
        Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId);
        Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId);
    }
}