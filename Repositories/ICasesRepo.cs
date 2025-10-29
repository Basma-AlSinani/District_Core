using Crime.Models;

namespace Crime.Repositories
{
    public interface ICasesRepo : IGenericRepository<Cases>
    {
        Task<Cases> GetCaseByNumberAsync(string caseNumber);
        IQueryable<Cases> GetAllQueryable(); // Get all cases as a queryable
        Task<Cases> GetCaseDetailsByIdAsync(int id); // Get case details by ID with related data

    }
}