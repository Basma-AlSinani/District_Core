using CrimeManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Repositories
{
    public class CaseParticipantsRepo : GenericRepository<CaseParticipants>, ICaseParticipantsRepo
    {
        private readonly CrimeDbContext _context;
        public CaseParticipantsRepo(CrimeDbContext context): base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaseParticipants>> GetByCaseIdAsync(int caseId)
        {
            return await _context.CaseParticipants
                .Include(cp => cp.Participant)
                .Include(cp => cp.AddedByUser)
                .Where(cp => cp.CaseId == caseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CaseParticipants>> GetByParticipantIdAsync(int participantId)
        {
            return await _context.CaseParticipants
                .Include(cp => cp.Case)
                .Where(cp => cp.ParticipantId == participantId)
                .ToListAsync();
        }
    }
}
