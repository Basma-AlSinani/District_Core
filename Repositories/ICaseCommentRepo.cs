using CrimeManagment.Models;

namespace CrimeManagment.Repositories
{
    public interface ICaseCommentRepo
    {
        Task AddCommentAsync(CaseComment comment);
        Task<int> CountUserCommentsInLastMinuteAsync(int userId);
        Task DeleteCommentAsync(int commentId, int userId);
        Task<IEnumerable<CaseComment>> GetCommentsByCaseIdAsync(int caseId);
    }
}