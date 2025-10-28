using Crime.Models;
using Crime.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crime.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantsRepo _participantRepository;
        public ParticipantService(IParticipantsRepo participantRepository)
        {
            _participantRepository = participantRepository;
        }

        // Get all participants
        public async Task<IEnumerable<Participants>> GetAllAsync()
        {
            return await _participantRepository.GetAllAsync();
        }

        // Get participant by ID
        public async Task<Participants> GetByIdAsync(int id)
        {
            return await _participantRepository.GetByIdAsync(id);
        }

        // Create a new participant
        public async Task<Participants> CreateAsync(Participants participant)
        {
            await _participantRepository.AddAsync(participant);
            await _participantRepository.SaveChangesAsync();
            return participant;
        }

        // Update an existing participant
        public async Task UpdateAsync(Participants participant)
        {
            await _participantRepository.UpdateAsync(participant);
            await _participantRepository.SaveChangesAsync();
        }

        // Delete a participant by ID
        public async Task DeleteAsync(int id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant != null)
            {
                await _participantRepository.DeleteAsync(participant);
                await _participantRepository.SaveChangesAsync();
            }
        }

        // Get participant by phone
        public async Task<Participants> GetByPhoneAsync(string phone)
        {
            return await _participantRepository.GetByPhoneAsync(phone);
        }
    }
}

