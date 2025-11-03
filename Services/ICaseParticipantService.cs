using CrimeManagment.Models;

namespace CrimeManagment.Services
{
    public interface ICaseParticipantService
    {
        Task<CaseParticipants> AddParticipantToCaseAsync(CaseParticipants cp);
        Task<IEnumerable<CaseParticipants>> GetByCaseIdAsync(int caseId);
        Task<IEnumerable<CaseParticipants>> GetByParticipantIdAsync(int participantId);
        Task RemoveAsync(int id);
    }
}