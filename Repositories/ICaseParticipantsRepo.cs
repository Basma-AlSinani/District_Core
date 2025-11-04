using Crime.Models;
using System.Linq.Expressions;

namespace Crime.Repositories
{
    public interface ICaseParticipantsRepo : IGenericRepository<CaseParticipants>
    {
        Task<IEnumerable<CaseParticipants>> GetByCaseIdAsync(int caseId);
        Task<IEnumerable<CaseParticipants>> GetByParticipantIdAsync(int participantId);
    }
}