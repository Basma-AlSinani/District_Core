using Crime.Models;

namespace Crime.Repositories
{
    public interface IParticipantsRepo : IGenericRepository<Participants>
    {
        Task<Participants> GetByPhoneAsync(string phone);
    }
}