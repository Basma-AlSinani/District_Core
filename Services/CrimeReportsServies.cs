using CrimeManagment.Repositories;
using CrimeManagment.Models;
namespace CrimeManagment.Services
{
    public class CrimeReportsServies : ICrimeReportsServies
    {
        private readonly ICrimeReportsRepository _repo;
        public CrimeReportsServies(ICrimeReportsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CrimeReports>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<CrimeReports> GetByIdAsync(int id)
        {
            var report = await _repo.GetByIdAsync(id);
            if (report == null)
                throw new Exception("Report not found");
            return report;
        }
        public async Task<IEnumerable<CrimeReports>> GetReportsByUserIdAsync(int userId)
        {
            return await _repo.GetReportsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<CrimeReports>> SearchAsync(string keyword)
        {
            return await _repo.SearchAsync(keyword);
        }

        public async Task AddAsync(CrimeReports report)
        {
            await _repo.AddAsync(report);
        }

        public async Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus)
        {
            await _repo.UpdateReportStatusAsync(reportId, newStatus);
        }

        public async Task DeleteAsync(int id)
        {
            var report = await _repo.GetByIdAsync(id);
            if (report == null)
                throw new Exception("Report not found");

            await _repo.DeleteAsync(report);
        }

    }
}
