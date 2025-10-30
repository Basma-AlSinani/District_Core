
namespace Crime.Services
{
    public interface ICaseCommentService
    {
        Task AddCommentAsync(int caseId, int userId, string content);
        Task DeleteCommentAsync(int commentId, int userId);
        Task<IEnumerable<object>> GetCommentsByCaseIdAsync(int caseId);
    }
}