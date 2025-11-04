using CrimeManagment.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrimeManagment.Repositories
{
    public class CrimeReportsRepo : GenericRepository<CrimeReports>, ICrimeReportsRepository
    {
        private readonly CrimeDbContext _context;
        public CrimeReportsRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        //get reports by user
        public async Task<IEnumerable<CrimeReports>> GetReportsByUserIdAsync(int userId)
        {
            return await _context.CrimeReports
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        //search reports by keyword in title or description
        public async Task<IEnumerable<CrimeReports>> SearchAsync(string keyword)
        {
            return await _context.CrimeReports
                .Where(r => r.Title.Contains(keyword) || r.Description.Contains(keyword))
                .ToListAsync();
        }

        //update report status
        public async Task UpdateReportStatusAsync(int reportId, CrimeStatus newStatus)
        {
            var report = await GetByIdAsync(reportId);
            if (report != null)
            {
                report.CrimeStatus = newStatus;
                await _context.SaveChangesAsync();
            }
        }
    }
}
