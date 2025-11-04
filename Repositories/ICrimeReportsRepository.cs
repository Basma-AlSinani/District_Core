using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICrimeReportsRepository : IGenericRepository<CrimeReports>
    {
        Task<IEnumerable<CrimeReports>> GetReportsByUserIdAsync(int userId);
        Task<IEnumerable<CrimeReports>> SearchAsync(string keyword);
        Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus);
    }
}