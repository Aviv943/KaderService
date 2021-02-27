using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using KaderService.Services.Data;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Authorization;

namespace KaderService.Controllers
{
    [Authorize]
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly KaderContext _context;
        private readonly PostsService _service;

        public PostsController(KaderContext context, PostsService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<GetPostsResponse>> GetPostsAsync()
        {
            IEnumerable<Post> posts = await _service.GetPostsAsync();

            var response = new GetPostsResponse
            {
                Posts = posts
            };

            return Ok(response);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostResponse>> GetPostAsync(string id)
        {
            var post = await _service.GetPostAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var response = new GetPostResponse
            {
                Post = post
            };

            return Ok(response);
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostAsync(string id, Post post)
        {
            await _service.UpdatePostAsync(id, post);
            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePostAsync(Post post)
        {
            await _service.CreatePostAsync(post);

            return CreatedAtAction("GetPostAsync", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            await _service.DeletePostAsync(id);
            return NoContent();
        }
    }
}