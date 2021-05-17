using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using KaderService.Logger;
using KaderService.ML.DTO;
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
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;

        public PostsController(PostsService service, UserManager<User> userManager, ILoggerManager logger) 
            : base(userManager)
        {
            _service = service;
            _userManager = userManager;
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
        public async Task<ActionResult<List<PostView>>> GetPostsAsync([FromQuery] string userId)
        {
            User user = await GetRelevantUserAsync(userId);
            List<Post> posts = await _service.GetPostsAsync(user);

            return posts.Select(p => new PostView
            {
                Creator = new UserView
                {
                    UserId = p.Creator.Id,
                    UserName = p.Creator.UserName,
                    FirstName = p.Creator.FirstName,
                    LastName = p.Creator.LastName,
                    Rating = p.Creator.Rating,
                    NumberOfRating = p.Creator.NumberOfRatings,
                    ImageUri = p.Creator.ImageUri
                },
                Created = p.Created,
                GroupId = p.GroupId,
                GroupName = p.Group.Name,
                Type = p.Type,
                Category = p.Category,
                PostId = p.PostId,
                Title = p.Title,
                Description = p.Description,
                Location = p.Address,
                ImagesUri = p.ImagesUri,
                CommentsCount = p.Comments.Count,
                Comments = new List<CommentView>(p.Comments.Select(comment => new CommentView
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Created = comment.Created,
                    Creator = new UserView
                    {
                        FirstName = comment.Creator.FirstName,
                        LastName = comment.Creator.LastName,
                        ImageUri = comment.Creator.ImageUri
                    }
                }))
            }).ToList();
        }

        [HttpGet("{userId}/recommended")]
        public async Task<ActionResult<IEnumerable<GetPostsResponse>>> GetRecommendedPostsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("UserId cannot be null");
            }

            IEnumerable<Post> postsForUserAsync = await _service.GetRecommendedPostsAsync(LoggedInUser);
            IEnumerable<PostView> postViews = postsForUserAsync.Select(p => new PostView
            {
                Creator = new UserView
                {
                    UserId = p.Creator.Id,
                    UserName = p.Creator.UserName,
                    FirstName = p.Creator.FirstName,
                    LastName = p.Creator.LastName,
                    Rating = p.Creator.Rating,
                    NumberOfRating = p.Creator.NumberOfRatings
                },
                Created = p.Created,
                GroupId = p.GroupId,
                GroupName = p.Group.Name,
                Type = p.Type,
                Category = p.Category,
                PostId = p.PostId,
                Title = p.Title,
                Description = p.Description,
                Location = p.Address,
                ImagesUri = p.ImagesUri,
                CommentsCount = p.Comments.Count
            });

            return Ok(new GetPostsResponse
            {
                PostViews = postViews.ToList()
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