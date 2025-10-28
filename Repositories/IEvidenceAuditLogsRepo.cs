using Crime.Models;

namespace Crime.Repositories
{
    public interface IEvidenceAuditLogsRepo : IGenericRepository<EvidenceAuditLogs>
    {
        Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId);
    }
}