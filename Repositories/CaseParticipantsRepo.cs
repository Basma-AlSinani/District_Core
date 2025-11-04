using Crime.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crime.Repositories
{
    public class CaseParticipantsRepo : GenericRepository<CaseParticipants>, ICaseParticipantsRepo
    {
        private readonly CrimeDbContext _context;
        public CaseParticipantsRepo(CrimeDbContext context): base(context)
        {
            _context = context;
        }

        // Get CaseParticipants by CaseId
        public async Task<IEnumerable<CaseParticipants>> GetByCaseIdAsync(int caseId)
        {
            return await _context.CaseParticipants
                .Include(cp => cp.Participant)
                .Include(cp => cp.AddedByUser)
                .Where(cp => cp.CaseId == caseId)
                .ToListAsync();
        }

        // Get CaseParticipants by ParticipantId
        public async Task<IEnumerable<CaseParticipants>> GetByParticipantIdAsync(int participantId)
        {
            return await _context.CaseParticipants
                .Include(cp => cp.Case)
                .Where(cp => cp.ParticipantId == participantId)
                .ToListAsync();
        }

        // Check if any CaseParticipants match the given predicate
        // Expression<Func<CaseParticipants -- means we can pass any condition we want to check
        public async Task<bool> AnyAsync(Expression<Func<CaseParticipants, bool>> predicate)
        {
            return await _context.CaseParticipants.AnyAsync(predicate);
        }
    }
}
