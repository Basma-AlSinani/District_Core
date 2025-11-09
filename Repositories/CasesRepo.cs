using CrimeManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Repositories
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

        // Get all cases 
        public async Task<IEnumerable<Cases>> GetAllAsync()
        {
            return await _context.Cases
                .Include(c => c.CreatedByUser)
                .ToListAsync();
        }


        // Get case details by ID with related data
        public async Task<Cases> GetCaseDetailsByIdAsync(int id)
        {
            return await _context.Cases
                .Include(c => c.CaseParticipants)
                    .ThenInclude(cp => cp.Participant)
                .Include(c => c.CaseReports)
                    .ThenInclude(cr => cr.CrimeReports)
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.CaseId == id);

        }

        public async Task<IEnumerable<Users>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _context.CaseAssignees
        .Where(a => a.CaseId == caseId)
        .Include(a => a.AssignedTo)
        .Select(a => a.AssignedTo)
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


