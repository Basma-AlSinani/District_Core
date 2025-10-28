using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class CaseAssigneesRepo : GenericRepository<CaseAssignees>, ICaseAssigneesRepo
    {
        private readonly CrimeDbContext _context;
        public CaseAssigneesRepo(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        //get all for a specific case 
        public async Task<IEnumerable<CaseAssignees>> GetAssigneesByCaseIdAsync(int caseId)
        {
            return await _dbSet
                .Include(ca => ca.Users)
                .Where(ca => ca.CaseId == caseId)
                .ToListAsync();
        }

        //Assign user to case 
        public async Task<bool> AssignUserToCaseAsync(int caseId, int userId, AssigneeRole role)
        {
            var caseEntity = await _context.Cases.FindAsync(caseId);
            var user = await _context.Users.FindAsync(userId);

            if (caseEntity == null || user == null)
            {
                return false; // Case or User does not exist
            }
            // Check clearance level for Officer role

            if (role == AssigneeRole.Officer && (int)user.ClearanceLevel < (int)caseEntity.AuthorizationLevel)
                return false;
            // Check if the user is already assigned to the case
            var assignment = new CaseAssignees
            {
                CaseId = caseId,
                UserId = userId,
                AssigneeRole = role,
                ProgreessStatus = ProgreessStatus.Pending,
                AssignedAt = DateTime.UtcNow
            };

            // Add the assignment
            await AddAsync(assignment);
            return true;// Assignment successful
        }
        //update assignee status
        public async Task<bool> UpdateAssigneeStatusAsync(int caseAssigneeId, ProgreessStatus newStatus)
        {
            // Find the assignment
            var assignment = await _dbSet.FindAsync(caseAssigneeId);
            // If not found, return false
            if (assignment == null)
            {
                return false; // Assignment not found
            }
            // Update the status
            assignment.ProgreessStatus = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

