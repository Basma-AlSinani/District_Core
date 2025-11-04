using CrimeManagment.Models;
using System.Linq.Expressions;

namespace CrimeManagment.Repositories
{
    public interface IParticipantsRepo : IGenericRepository<Participants>
    {
        Task<Participants> GetByPhoneAsync(string phone);
        Task<bool> AnyAsync(Expression<Func<Participants, bool>> predicate);
    }
}