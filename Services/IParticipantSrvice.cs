using CrimeManagment.DTOs;

namespace CrimeManagment.Services
{
    public interface IParticipantService
    {
        Task<ParticipantDto?> CreateAsync(CreateParticipantDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ParticipantDto>> GetAllAsync();
        Task<ParticipantDto?> GetByIdAsync(int id);
        Task<ParticipantDto?> GetByPhoneAsync(string phone);
        Task<bool> UpdateAsync(int id, ParticipantDto dto);
    }
}