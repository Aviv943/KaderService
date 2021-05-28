using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using KaderService.Services.Constants;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Controllers
{
    //[Authorize]
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : MyControllerBase
    {
        private readonly CommentsService _service;

        public CommentsController(CommentsService service, UserManager<User> userManager)
            : base(userManager)
        {
            _service = service;
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<IEnumerable<CommentView>>> GetCommentsAsync(string postId, [FromQuery] PagingParameters paging)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                return BadRequest("PostNumber cannot be null");
            }

            IEnumerable<CommentView> commentViews = await _service.GetCommentsAsync(postId, paging);

            return Ok(new GetCommentsResponse
            {
                CommentViews = commentViews.ToList()
            });
        }

        // GET: api/Comments/5
        [HttpGet("comment/{id}")]
        public async Task<ActionResult<Comment>> GetCommentAsync(string id)
        {
            Comment comment = await _service.GetCommentAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommentAsync(string id, Comment comment)
        {
            await _service.UpdateCommentAsync(id, comment);
            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{postId}")]
        public async Task<ActionResult<Comment>> CreateCommentAsync(Comment comment, string postId)
        {
            await _service.CreateCommentAsync(comment, LoggedInUser, postId);
            return CreatedAtAction("GetCommentsAsync", new { postId }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentAsync(string id)
        {
            await _service.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}