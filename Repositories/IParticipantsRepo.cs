using Crime.Models;
using System.Linq.Expressions;

namespace Crime.Repositories
{
    public interface IParticipantsRepo : IGenericRepository<Participants>
    {
        Task<Participants> GetByPhoneAsync(string phone);
        Task<bool> AnyAsync(Expression<Func<Participants, bool>> predicate);
    }
}