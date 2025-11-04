using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface IEvidenceAuditLogsRepo : IGenericRepository<EvidenceAuditLogs>
    {
        Task<IEnumerable<EvidenceAuditLogs>> GetLogsByEvidenceIdAsync(int evidenceId);
    }
}