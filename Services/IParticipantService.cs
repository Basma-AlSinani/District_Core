using Crime.Models;

namespace Crime.Services
{
    public interface IParticipantService
    {
        Task<Participants> CreateAsync(Participants participant);
        Task DeleteAsync(int id);
        Task<IEnumerable<Participants>> GetAllAsync();
        Task<Participants> GetByIdAsync(int id);
        Task<Participants> GetByPhoneAsync(string phone);
        Task UpdateAsync(Participants participant);
    }
}