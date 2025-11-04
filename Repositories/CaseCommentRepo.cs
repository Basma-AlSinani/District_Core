using CrimeManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeManagment.Repositories
{
    public class CaseCommentRepo : ICaseCommentRepo
    {
        private readonly CrimeDbContext _context;
        public CaseCommentRepo(CrimeDbContext context)
        {
            _context = context;
        }

        // Add a new comment
        public async Task AddCommentAsync(CaseComment comment)
        {
            _context.CaseComments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        // Get comments by case ID with user details
        public async Task<IEnumerable<CaseComment>> GetCommentsByCaseIdAsync(int caseId)
        {
            return await _context.CaseComments
                .Include(c => c.User)
                .Where(c => c.CaseId == caseId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        // Delete comment if it belongs to the user
        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _context.CaseComments
              .FirstOrDefaultAsync(c => c.CommentId == commentId && c.UserId == userId);

            if (comment != null)
            {
                _context.CaseComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        // Count comments by user in the last minute
        public async Task<int> CountUserCommentsInLastMinuteAsync(int userId)
        {
            // Calculate the timestamp for one minute ago
            var oneMinuteAgo = DateTime.UtcNow.AddMinutes(-1);

            // Query the database to count comments by the user created after that timestamp
            return await _context.CaseComments
                .CountAsync(c => c.UserId == userId && c.CreatedAt >= oneMinuteAgo);
        }
    }
}
