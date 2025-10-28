
namespace Crime.Repositories
{
    public interface IEvidenceAuditLogs : IGenericRepository<EvidenceAuditLogs>
    {
        Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId);
    }
}