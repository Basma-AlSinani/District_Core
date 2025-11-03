using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICaseParticipantsRepo : IGenericRepository<CaseParticipants>
    {
        Task<IEnumerable<CaseParticipants>> GetByCaseIdAsync(int caseId);
        Task<IEnumerable<CaseParticipants>> GetByParticipantIdAsync(int participantId);
    }
}