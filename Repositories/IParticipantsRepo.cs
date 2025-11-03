using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface IParticipantsRepo : IGenericRepository<Participants>
    {
        Task<Participants> GetByPhoneAsync(string phone);
    }
}