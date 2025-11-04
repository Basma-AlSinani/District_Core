using CrimeManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Repositories
{
    public class CaseReportRepo : GenericRepository<CaseReports>, ICaseReportRepo
    {
        private readonly CrimeDbContext _context;
        public CaseReportRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        // Check if Case is already linked with this Report
        public async Task<bool> ExistsAsync(int caseId, int crimeReportId)
        {
            return await _context.CaseReports
                .AnyAsync(x => x.CaseId == caseId && x.CrimeReportId == crimeReportId);
        }


        // Get specific link (details)
        public async Task<CaseReports?> GetDetailsAsync(int id)
        {
            return await _context.CaseReports
                .Include(x => x.cases)
                .Include(x => x.CrimeReports)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.CaseReportId == id);
        }

        // Get CaseReports by CaseId
        public async Task<IEnumerable<CaseReports>> GetByCaseIdAsync(int caseId)
        {
            return await _context.CaseReports
                .Where(x => x.CaseId == caseId)
                .Include(x => x.CrimeReports)
                .Include(x => x.Users)
                .ToListAsync();
        }

        // Get CaseReports by ReportId
        public async Task<IEnumerable<CaseReports>> GetByReportIdAsync(int reportId)
        {
            return await _context.CaseReports
                .Where(x => x.CrimeReportId == reportId)
                .Include(x => x.cases)
                .Include(x => x.Users)
                .ToListAsync();
        }

        // Get all relations
        public async Task<IEnumerable<CaseReports>> GetAllWithRelationsAsync()
        {
            return await _context.CaseReports
                .Include(x => x.cases)
                .Include(x => x.CrimeReports)
                .Include(x => x.Users)
                .AsNoTracking()
                .ToListAsync();
        }

        // Search CaseReports by Case Name, Report Title or Notes
        public async Task<IEnumerable<CaseReports>> SearchCasesAsync(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _context.CaseReports
                    .Include(x => x.cases)
                    .Include(x => x.CrimeReports)
                    .Include(x => x.Users)
                    .ToListAsync();

            keyword = keyword.ToLower();

            return await _context.CaseReports
                .Include(x => x.cases)
                .Include(x => x.CrimeReports)
                .Include(x => x.Users)
                .Where(x =>
                    (x.cases.Name != null && x.cases.Name.ToLower().Contains(keyword)) ||
                    (x.CrimeReports.Title != null && x.CrimeReports.Title.ToLower().Contains(keyword)) ||
                    (x.Notes != null && x.Notes.ToLower().Contains(keyword))
                )
                .ToListAsync();
        }

        public async Task AddAsync(CaseReports entity)
        {
            await _context.CaseReports.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CaseReports entity)
        {
            _context.CaseReports.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CaseReports entity)
        {
            _context.CaseReports.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CaseReports> GetByIdAsync(int id)
        {
            return await _context.CaseReports.FindAsync(id);
        }

        public async Task<CaseReports> GetByCaseAndReportAsync(int caseId, int crimeReportId)
        {
            return await _context.CaseReports
                .FirstOrDefaultAsync(x => x.CaseId == caseId && x.CrimeReportId == crimeReportId);
        }

    }
}

