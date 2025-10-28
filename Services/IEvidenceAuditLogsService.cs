using Crime.Models;

namespace Crime.Services
{
    public interface IEvidenceAuditLogsService
    {
        Task AddAsync(EvidenceAuditLogs log);
        Task<IEnumerable<EvidenceAuditLogs>> GetAllAsync();
        Task<EvidenceAuditLogs> GetByIdAsync(int id);
        Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId);
    }
}