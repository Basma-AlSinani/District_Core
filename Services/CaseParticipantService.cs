using Crime.Models;
using Crime.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crime.Services
{
    public class CaseParticipantService : ICaseParticipantService
    {
        private readonly ICaseParticipantsRepo _caseParticipantRepository;

        public CaseParticipantService(ICaseParticipantsRepo caseParticipantRepository)
        {
            _caseParticipantRepository = caseParticipantRepository;
        }

        // Get all participants for a specific case
        public async Task<IEnumerable<CaseParticipants>> GetByCaseIdAsync(int caseId)
        {
            return await _caseParticipantRepository.GetByCaseIdAsync(caseId);
        }

        // Get all cases for a specific participant
        public async Task<IEnumerable<CaseParticipants>> GetByParticipantIdAsync(int participantId)
        {
            return await _caseParticipantRepository.GetByParticipantIdAsync(participantId);
        }

        // Add a participant to a case
        public async Task<CaseParticipants> AddParticipantToCaseAsync(CaseParticipants cp)
        {
            await _caseParticipantRepository.AddAsync(cp);
            await _caseParticipantRepository.SaveChangesAsync();
            return cp;
        }

        // Remove a participant from a case
        public async Task RemoveAsync(int id)
        {
            var caseParticipant = await _caseParticipantRepository.GetByIdAsync(id);
            if (caseParticipant != null)
            {
                await _caseParticipantRepository.DeleteAsync(caseParticipant);
                await _caseParticipantRepository.SaveChangesAsync();
            }
        }
    }
}

