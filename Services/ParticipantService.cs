using AutoMapper;
using CrimeManagment.DTOs;
using CrimeManagment.Models;
using CrimeManagment.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrimeManagment.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantsRepo _participantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ParticipantService> _logger;

        public ParticipantService(IParticipantsRepo participantRepository, IMapper mapper, ILogger<ParticipantService> logger)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // Get all participants
        public async Task<IEnumerable<ParticipantDto>> GetAllAsync()
        {
            var data = await _participantRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ParticipantDto>>(data);
        }

        // Get participant by ID
        public async Task<ParticipantDto?> GetByIdAsync(int id)
        {
            var entity = await _participantRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ParticipantDto>(entity);
        }

        // Create a new participant
        public async Task<ParticipantDto?> CreateAsync(CreateParticipantDto dto)
        {
            try
            {
                var entity = _mapper.Map<Participants>(dto);

                var now = DateTime.UtcNow;
                var participantDto = _mapper.Map<ParticipantDto>(entity);
                participantDto.AddedOn = now;
                participantDto.AddedByUserId = dto.AddedByUserId;

                await _participantRepository.AddAsync(entity);
                await _participantRepository.SaveChangesAsync();

                _logger.LogInformation("Participant {Name} added successfully.", entity.FullName);

                return participantDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating participant.");
                return null;
            }
        }



        // Update an existing participant
        public async Task<bool> UpdateAsync(int id, ParticipantDto dto)
        {
            var existing = await _participantRepository.GetByIdAsync(id);
            if (existing == null)
                return false;

            _mapper.Map(dto, existing);
            await _participantRepository.UpdateAsync(existing);
            await _participantRepository.SaveChangesAsync();
            return true;
        }

        // Delete a participant by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null)
                return false;

            await _participantRepository.DeleteAsync(participant);
            await _participantRepository.SaveChangesAsync();
            return true;
        }

        // Get participant by phone
        public async Task<ParticipantDto?> GetByPhoneAsync(string phone)
        {
            var entity = await _participantRepository.GetByPhoneAsync(phone);
            return entity == null ? null : _mapper.Map<ParticipantDto>(entity);
        }
    }
}
