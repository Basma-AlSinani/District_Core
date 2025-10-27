using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class EvidenceAuditLogs : GenericRepository<EvidenceAuditLogs>, IEvidenceAuditLogs
    {
        private readonly CrimeDbContext _context;
        public EvidenceAuditLogs(CrimeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId)
        {
            return (IEnumerable<EvidenceAuditLogs>)await _context.EvidenceAuditLogs
                .Where(e => e.EvidenceItemId == evidenceId)
                .ToListAsync();
        }
    }
}
