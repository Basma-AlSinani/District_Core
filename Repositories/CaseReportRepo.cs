using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class CaseReportRepo : GenericRepository<CaseReports>, ICaseReportRepo
    {
        private readonly CrimeDbContext _context; 
        public CaseReportRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        // Get CaseReports by CaseId and CrimeReportId
        public async Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId)
        {
            return await _context.CaseReports
                .FirstOrDefaultAsync(x => x.CaseId == caseId && x.CrimeReportId == crimeReportId);
        }

        // Get CaseReports by CaseId
        public async Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId)
        {
            return await _context.CaseReports
                .Where(x => x.CaseId == caseId)
                .Include(x => x.CrimeReports)
                .Include(x => x.PerformedBy)
                .ToListAsync();
        }

        // Get CaseReports by ReportId
        public async Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId)
        {
            return await _context.CaseReports
                .Where(x => x.CrimeReportId == reportId)
                .Include(x => x.CrimeReports)
                .Include(x => x.Users)
                .ToListAsync();
        }
    }
}
