using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using KaderService.Logger;
using KaderService.ML.DTO;
using KaderService.Services.Constants;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace KaderService.Controllers
{
    //[Authorize]
    [Route("api/posts")]
    [ApiController]
    public class PostsController : MyControllerBase
    {
        private readonly PostsService _service;
        private readonly ILoggerManager _logger;

        public PostsController(PostsService service, UserManager<User> userManager, ILoggerManager logger) 
            : base(userManager)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<GetPostResponse>> GetPostAsync(string postId)
        {
            Post post = await _service.GetPostAsync(postId, LoggedInUser);

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
        
        [HttpGet]
        public async Task<ActionResult<List<PostView>>> GetPostsAsync([FromQuery] string userId, [FromQuery] PagingParameters paging)
        {
            User user = await GetRelevantUserAsync(userId);
            List<PostView> posts = await _service.GetPostsAsync(user, paging);

            return posts;
        }

        [HttpGet("{userId}/recommended")]
        public async Task<ActionResult<IEnumerable<GetPostsResponse>>> GetRecommendedPostsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("UserId cannot be null");
            }

            IEnumerable<PostView> postsAsync = await _service.GetRecommendedPostsAsync(LoggedInUser);

            return Ok(new GetPostsResponse
            {
                PostViews = postsAsync.ToList()
            });
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
            return CreatedAtAction("GetPostAsync", new { postId = post.PostId }, post);
        }

        [HttpPost("post/{postId}/image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GetPostResponse>> CreatePostImageAsync(string postId)
        {
            _logger.LogDebug($"[{LoggedInUser.UserName}] Request to create post image for postId {postId}");
            IFormFile file = Request.Form.Files.First();
            string serverImageUrl = await _service.CreatePostImageAsync(postId, LoggedInUser, file);

            return Ok(serverImageUrl);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            await _service.DeletePostAsync(id);
            return NoContent();
        }
    }
}