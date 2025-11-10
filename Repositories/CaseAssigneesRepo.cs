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
        //  Get all assignees for a specific case (with full navigation loading)
        public async Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _context.CaseAssignees
                .Include(ca => ca.Cases)
                .Include(ca => ca.AssignedTo)
                .Include(ca => ca.AssignedBy)
                .Where(ca => ca.CaseId == caseId)
                .ToListAsync();
        }
        //  Assign a user to a case (creates a new record)
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
            await _context.CaseAssignees.AddAsync(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
        //  Update the status of an existing assignee
        public async Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus)
        {
            var assignment = await _context.CaseAssignees.FindAsync(caseAssigneeId);
            if (assignment == null)
                return false;
            assignment.Status = newStatus;
            _context.CaseAssignees.Update(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
        //  Fetch user by ID (used for validation)
        public async Task<Users?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        //  Get case authorization level for permission checks
        public async Task<int> GetCaseAuthorizationLevel(int caseId)
        {
            var c = await _context.Cases.FindAsync(caseId);
            return c != null ? (int)c.AuthorizationLevel : 0;
        }
    }
}