using Crime.Repositories;
using Crime.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CaseCommentsControllers : ControllerBase
    {
        private readonly ICaseCommentService _commentService;

        public CaseCommentsControllers(ICaseCommentService commentService)
        {
            _commentService = commentService;
        }

        // Add a new comment to a case
        [HttpPost("Add/{caseId}/{userId}")]
        public async Task<IActionResult> AddComment(int caseId, int userId, [FromBody] string content)
        {
            try
            {
                await _commentService.AddCommentAsync(caseId, userId, content);
                return Ok(new { message = "Comment added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Get all comments for a specific case
        [HttpGet("GetAll/{caseId}")]
        public async Task<IActionResult> GetComments(int caseId)
        {
            var comments = await _commentService.GetCommentsByCaseIdAsync(caseId);
            return Ok(comments);
        }

        // Delete a comment if it belongs to the user
        [HttpDelete("Delete/{commentId}/{userId}")]
        public async Task<IActionResult> DeleteComment(int commentId, int userId)
        {
            await _commentService.DeleteCommentAsync(commentId, userId);
            return Ok(new { message = "Comment deleted successfully." });
        }
    }
}

