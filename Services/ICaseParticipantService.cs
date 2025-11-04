using CrimeManagment.DTOs;

namespace Crime.Services
{
    public interface ICaseParticipantService
    {
        Task<CaseParticipantDto?> AddAsync(CreateCaseParticipantDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CaseParticipantDto>> GetAllAsync();
        Task<CaseParticipantDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateCaseParticipantDto dto);
    }
}