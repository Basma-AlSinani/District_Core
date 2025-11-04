using CrimeManagment.Models;
using AutoMapper;
using CrimeManagment.DTOs;
using CrimeManagment.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Services
{
    public class CaseParticipantService : ICaseParticipantService
    {
        private readonly ICaseParticipantsRepo _caseParticipantRepo;
        private readonly IParticipantsRepo _participantsRepo;
        private readonly ICasesRepo _casesRepo;
        private readonly IMapper _mapper;

        public CaseParticipantService(ICaseParticipantsRepo caseParticipantRepo, IParticipantsRepo participantsRepo,
            ICasesRepo casesRepo, IMapper mapper)
        {
            _caseParticipantRepo = caseParticipantRepo;
            _participantsRepo = participantsRepo;
            _casesRepo = casesRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CaseParticipantDto>> GetAllAsync()
        {
            var data = await _caseParticipantRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<CaseParticipantDto>>(data);
        }

        // Get case participants by CaseId
        public async Task<CaseParticipantDto?> GetByIdAsync(int id)
        {
            var cp = await _caseParticipantRepo.GetByIdAsync(id);
            return cp == null ? null : _mapper.Map<CaseParticipantDto>(cp);
        }

        // Add a new case participant
        public async Task<CaseParticipantDto?> AddAsync(CreateCaseParticipantDto dto)
        {
            // Check duplicate
            var exists = await _caseParticipantRepo.GetAllAsync();
            var isDuplicate = exists.Any(cp => cp.CaseId == dto.CaseId && cp.ParticipantId == dto.ParticipantId);
            if (isDuplicate)
                return null;

            // Validate Case & Participant existence
            var caseExists = await _casesRepo.GetAllQueryable()
                .AnyAsync(c => c.CaseId == dto.CaseId);

            var participantExists = await _participantsRepo.AnyAsync(
                p => p.ParticipantsId == dto.ParticipantId
            );

            if (!caseExists || !participantExists)
                return null;

            var entity = _mapper.Map<CaseParticipants>(dto);
            entity.AssignedAt = DateTime.Now;

            await _caseParticipantRepo.AddAsync(entity);
            await _caseParticipantRepo.SaveChangesAsync();

            return _mapper.Map<CaseParticipantDto>(entity);
        }

        // Update existing case participant
        public async Task<bool> UpdateAsync(int id, UpdateCaseParticipantDto dto)
        {
            var existing = await _caseParticipantRepo.GetByIdAsync(id);
            if (existing == null) return false;

            _mapper.Map(dto, existing);

            await _caseParticipantRepo.UpdateAsync(existing);
            await _caseParticipantRepo.SaveChangesAsync();
            return true;
        }

        // Delete case participant
        public async Task<bool> DeleteAsync(int id)
        {
            var cp = await _caseParticipantRepo.GetByIdAsync(id);
            if (cp == null) return false;

            await _caseParticipantRepo.DeleteAsync(cp);
            await _caseParticipantRepo.SaveChangesAsync();
            return true;
        }
    }
}

