﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using KaderService.Services.Services;
using KaderService.Services.ViewModels;
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

        // GET: api/posts/post/{postId}
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<GetPostResponse>> GetPostAsync(string postId)
        {
            var post = await _service.GetPostAsync(postId, LoggedInUser);

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

        // GET: api/posts/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsAsync()
        {
            IEnumerable<Post> postsAsync = await _service.GetPostsAsync();

            return Ok(postsAsync);
        }

        // GET: api/posts/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<GetPostsResponse>>> GetPostsByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("UserId cannot be null");
            }

            IEnumerable<Post> postsForUserAsync = await _service.GetPostsByUserAsync(userId);
            IEnumerable<PostView> postViews = postsForUserAsync.Select(p => new PostView{
                UserView = new UserView
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
                Location = p.Location,
                ImagesUri = p.ImagesUri,
                CommentsCount = p.Comments.Count
            });

            return Ok(new GetPostsResponse
            {
                PostViews = postViews.ToList()
            });
        }

        // GET: api/posts/{userId}
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
                UserView = new UserView
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
                Location = p.Location,
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            await _service.DeletePostAsync(id);
            return NoContent();
        }
    }
}