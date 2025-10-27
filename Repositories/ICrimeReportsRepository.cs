using Crime.Models;

namespace Crime.Repositories
{
    public interface ICrimeReportsRepository
    {
        Task<IEnumerable<CrimeReports>> GetReportsByUserIdAsync(int userId);
        Task<IEnumerable<CrimeReports>> SearchAsync(string keyword);
        Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus);
    }
}