using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface ICrimeReportsServies
    {
        Task AddAsync(CrimeReports report);
        Task DeleteAsync(int id);
        Task<IEnumerable<CrimeReports>> GetAllAsync();
        Task<CrimeReports> GetByIdAsync(int id);
        Task<IEnumerable<CrimeReports>> GetReportsByUserIdAsync(int userId);
        Task<IEnumerable<CrimeReports>> SearchAsync(string keyword);
        Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus);
    }
}