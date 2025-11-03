using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICaseReportRepo : IGenericRepository<CaseReports>
    {
        Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId);
        Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId);
        Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId);
    }
}