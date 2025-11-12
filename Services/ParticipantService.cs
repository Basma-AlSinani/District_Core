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
        private readonly ICaseParticipantsRepo _caseParticipantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ParticipantService> _logger;

        public ParticipantService(IParticipantsRepo participantRepository, ICaseParticipantsRepo caseParticipantRepository, IMapper mapper, ILogger<ParticipantService> logger)
        {
            _participantRepository = participantRepository;
            _caseParticipantRepository = caseParticipantRepository;
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

                await _participantRepository.AddAsync(entity);
                await _participantRepository.SaveChangesAsync();

                var caseParticipant = new CaseParticipants
                {
                    CaseId = dto.CaseId,
                    ParticipantId = entity.ParticipantsId,
                    AddedByUserId = dto.AddedByUserId,
                    Notes = dto.Notes,
                    Role = Role.Witness, 
                    AssignedAt = DateTime.Now
                };

                await _caseParticipantRepository.AddAsync(caseParticipant);
                await _caseParticipantRepository.SaveChangesAsync();

                var participantDto = _mapper.Map<ParticipantDto>(entity);
                participantDto.AddedOn = DateTime.UtcNow;
                participantDto.AddedByUserId = dto.AddedByUserId;

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
