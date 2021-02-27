using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Data;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Authorization;

namespace KaderService.Controllers
{
    [Authorize]
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly KaderContext _context;
        private readonly CommentsService _service;

        public CommentsController(KaderContext context, CommentsService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsAsync()
        {
            return Ok(await _service.GetCommentsAsync());
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentAsync(string id)
        {
            var comment = await _service.GetCommentAsync(id);

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
        [HttpPost]
        public async Task<ActionResult<Comment>> PostCommentAsync(Comment comment)
        {
            await _service.CreateCommentAsync(comment);

            return CreatedAtAction("GetCommentsAsync", new { id = comment.Id }, comment);
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