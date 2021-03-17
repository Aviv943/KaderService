using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Controllers
{
    //[Authorize]
    [Route("api/posts")]
    [ApiController]
    public class PostsController : MyControllerBase
    {
        private readonly PostsService _service;

        public PostsController(PostsService service, UserManager<User> userManager) 
            : base(userManager)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsAsync()
        {
            return Ok(await _service.GetPostsAsync());
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsForUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("UserId cannot be null");
            }

            return Ok(await _service.GetPostsForUserAsync(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostResponse>> GetPostAsync(string id)
        {
            Post post = await _service.GetPostAsync(id);

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostAsync(string id, Post post)
        {
            await _service.UpdatePostAsync(id, post);
            return NoContent();
        }

        [HttpPost("post/{groupId}")]
        public async Task<ActionResult<Post>> CreatePostAsync(Post post, string groupId)
        {
            await _service.CreatePostAsync(post, LoggedInUser, groupId);
            return CreatedAtAction("GetPostAsync", new { id = post.PostId }, post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            await _service.DeletePostAsync(id);
            return NoContent();
        }
    }
}