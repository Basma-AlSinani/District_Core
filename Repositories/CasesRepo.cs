using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class CasesRepo : GenericRepository<Cases>, ICasesRepo
    {
        private readonly CrimeDbContext _context;
        public CasesRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Cases> GetCaseByNumberAsync(string caseNumber)
        {
            return await _context.Cases
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.CaseNumber == caseNumber);
        }

        // Get all cases as a queryable
        public IQueryable<Cases> GetAllQueryable()
        {
            return _context.Cases
                .Include(c => c.CreatedByUser)
                .AsQueryable();

        }

        // Get case details by ID with related data
        public async Task<Cases> GetCaseDetailsByIdAsync(int id)
        {
            return await _context.Cases
                .Include(c => c.CreatedByUser)
                .Include(c => c.CaseReports)
                .ThenInclude(cr => cr.CrimeReports)
                .ThenInclude(cr => cr.Users)
                .FirstOrDefaultAsync(c => c.CaseId == id);
        }

        public async Task<IEnumerable<Users>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _context.CaseAssignees
                .Where(a => a.CaseId == caseId)
                .Include(a => a.Users)
                .Select(a => a.Users)
                .ToListAsync();
        }

        public async Task<IEnumerable<Evidence>> GetEvidenceByCaseIdAsync(int caseId)
        {
            return await _context.Evidences
                .Where(e => e.CaseId == caseId && !e.IsSoftDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<CaseParticipants>> GetParticipantsByRoleAsync(int caseId, Role role)
        {
            return await _context.CaseParticipants
                .Where(p => p.CaseId == caseId && p.Role == role)
                .Include(p => p.Participant)
                .ToListAsync();
        }
    }
}


