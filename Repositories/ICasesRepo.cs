using Crime.Models;

namespace Crime.Repositories
{
    public interface ICasesRepo : IGenericRepository<Cases>
    {
        Task<Cases> GetCaseByNumberAsync(string caseNumber);
        IQueryable<Cases> GetAllQueryable();
    }
}