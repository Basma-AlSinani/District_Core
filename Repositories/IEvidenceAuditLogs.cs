
namespace Crime.Repositories
{
    public interface IEvidenceAuditLogs
    {
        Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId);
    }
}