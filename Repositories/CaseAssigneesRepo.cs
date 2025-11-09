using CrimeManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Repositories
{
    public class CaseAssigneesRepo : GenericRepository<CaseAssignees>, ICaseAssigneesRepo
    {
        private readonly CrimeDbContext _context;
        public CaseAssigneesRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _dbSet
                .Include(ca => ca.AssignedToUserId)
                .Where(ca => ca.CaseId == caseId)
                .ToListAsync();
        }

        public async Task<bool> AssignUserToCaseAsync(int caseId, int userId, AssigneeRole role)
        {
            var assignment = new CaseAssignees
            {
                CaseId = caseId,
                AssignedByUserId = userId,
                Role = role,
                Status = ProgreessStatus.Pending,
                AssignedAt = DateTime.UtcNow
            };

            await AddAsync(assignment);
            return true;
        }

        public async Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus)
        {
            var assignment = await _dbSet.FindAsync(caseAssigneeId);
            if (assignment == null) return false;
            assignment.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<int> GetCaseAuthorizationLevel(int caseId)
        {
            var c = await _context.Cases.FindAsync(caseId);
            return c != null ? (int)c.AuthorizationLevel : 0;
        }
    }
}
