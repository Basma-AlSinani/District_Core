using Crime.Models;
using Microsoft.EntityFrameworkCore;

namespace Crime.Repositories
{
    public class EvidenceAuditLogsRepo : GenericRepository<EvidenceAuditLogs>, IEvidenceAuditLogsRepo
    {
        private readonly CrimeDbContext _context;
        public EvidenceAuditLogsRepo(CrimeDbContext context) : base(context)
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
