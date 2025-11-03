using CrimeManagment.Models;
using CrimeManagment.Repositories;
using System.Text.RegularExpressions;

namespace CrimeManagment.Services
{
    public class CaseCommentService : ICaseCommentService
    {
        private readonly ICaseCommentRepo _caseCommentRepo;

        // Allowed characters: letters, numbers, spaces, and basic punctuation
        private readonly Regex allowedCharsRegex = new(@"^[a-zA-Z0-9 .,?!'""-]+$");

        public CaseCommentService(ICaseCommentRepo caseCommentRepo)
        {
            _caseCommentRepo = caseCommentRepo;
        }

        // Add a new comment with validation and rate limiting
        public async Task AddCommentAsync(int caseId, int userId, string content)
        {
            // Validate length
            if (content.Length < 5)
                throw new Exception("Comment must be at least 5 characters long.");
            if (content.Length > 150)
                throw new Exception("Comment cannot exceed 150 characters.");

            // Disallow HTML tags
            if (Regex.IsMatch(content, @"<[^>]+>"))
                throw new Exception("HTML tags are not allowed in comments.");

            // Restrict to allowed characters
            if (!allowedCharsRegex.IsMatch(content))
                throw new Exception("Comment contains invalid characters. Please use only letters, numbers, and basic punctuation.");

            // Rate limiting: max 5 comments per minute
            int recentCount = await _caseCommentRepo.CountUserCommentsInLastMinuteAsync(userId);
            if (recentCount >= 5)
                throw new Exception("You can post a maximum of 5 comments per minute.");
            // Create and save the comment
            var comment = new CaseComment
            {
                CaseId = caseId,
                UserId = userId,
                Content = content
            };

            // Save the comment
            await _caseCommentRepo.AddCommentAsync(comment);
        }

        // Get comments for a case with user details
        public async Task<IEnumerable<object>> GetCommentsByCaseIdAsync(int caseId)
        {
            // Retrieve comments from the repository
            var comments = await _caseCommentRepo.GetCommentsByCaseIdAsync(caseId);
            // Map to desired output format
            return comments.Select(c => new
            {
                c.CommentId,
                c.Content,
                User = $"{c.User.FirstName} {c.User.LastName}",
                c.CreatedAt
            });
        }

        // Delete a comment if it belongs to the user
        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            await _caseCommentRepo.DeleteCommentAsync(commentId, userId);
        }
    }
}
