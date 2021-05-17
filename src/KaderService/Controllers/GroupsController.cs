﻿using System;
using System.Collections.Generic;
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
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : MyControllerBase
    {
        private readonly GroupsService _service;

        public GroupsController(GroupsService service, UserManager<User> userManager) 
            : base(userManager)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = "GroupManager")]
        public async Task<ActionResult<GroupView>> GetGroupAsync(string id)
        {
            Group group = await _service.GetGroupAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            var posts = new List<PostView>();

            foreach (Post post in group.Posts)
            {
                List<CommentView> comments = post.Comments.Select(comment => new CommentView
                {
                    Creator = new UserView
                    {
                        UserId = post.Creator.Id,
                        FirstName = post.Creator.FirstName,
                        LastName = post.Creator.LastName,
                        ImageUri = post.Creator.ImageUri
                    },
                    CommentId = comment.CommentId,
                    Content = comment.Content, 
                    Created = comment.Created
                }).ToList();

                posts.Add(new PostView
                {
                    Creator = new UserView
                    {
                        UserId = post.Creator.Id,
                        FirstName = post.Creator.FirstName,
                        LastName = post.Creator.LastName,
                        ImageUri = post.Creator.ImageUri
                    },
                    PostId = post.PostId,
                    Category = post.Category,
                    Comments = comments,
                    Created = post.Created,
                    Description = post.Description,
                    CommentsCount = comments.Count,
                    ImagesUri = post.ImagesUri,
                    Location = post.Location,
                    Title = post.Title,
                    Type = post.Type
                });
            }

            List<UserView> members = group.Members.Select(member => new UserView
                {
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    ImageUri = member.ImageUri,
                    UserId = member.Id,
                    Rating = member.Rating,
                    NumberOfRating = member.NumberOfRatings
                }).ToList();

            List<UserView> managers = group.Managers.Select(manager => new UserView
                {
                    FirstName = manager.FirstName,
                    LastName = manager.LastName,
                    ImageUri = manager.ImageUri,
                    UserId = manager.Id,
                    Rating = manager.Rating,
                    NumberOfRating = manager.NumberOfRatings
                }).ToList();

            return Ok(new GroupView
            {
                Name = group.Name,
                Category = group.Category,
                Created = group.Created,
                Description = group.Description,
                GroupId = group.GroupId,
                GroupPrivacy = group.GroupPrivacy,
                Address = group.Address,
                Managers = managers,
                Members = members,
                PostsCount = group.Posts.Count,
                Posts = posts,
            });
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<GroupView>>> GetGroupsAsync([FromQuery] string userId)
        {
            User user = await GetRelevantUserAsync(userId);

            IEnumerable<Group> groups = await _service.GetGroupsAsync(user);
            IEnumerable<GroupView> groupsViews = groups.Select(group =>
            {
                User userManager = group.Managers.FirstOrDefault(us => us.Id == user.Id);

                return new GroupView
                {
                    Name = group.Name,
                    Category = group.Category,
                    Created = group.Created,
                    Description = group.Description,
                    GroupId = group.GroupId,
                    GroupPrivacy = group.GroupPrivacy,
                    Address = group.Address,
                    ManagersCount = group.Managers.Count,
                    MembersCount = group.Members.Count,
                    PostsCount = group.Posts.Count,
                    IsManager = userManager != null
                };
            });

            return Ok(groupsViews);
        }

        [HttpGet("{id}/posts")]
        public async Task<ActionResult<ICollection<Post>>> GetGroupPostsByIdAsync(string id)
        {
            ICollection<Post> groupPosts = await _service.GetGroupPostsByIdAsync(id);

            if (groupPosts == null)
            {
                return NotFound();
            }

            return Ok(groupPosts);
        }

        [HttpGet("search")]
        //[Authorize(Policy = "GroupManager")]
        public async Task<ActionResult<GetGroupResponse>> SearchGroupsAsync(string text, string category, string location)
        {
            Group group = await _service.GetGroupAsync("");

            if (group == null)
            {
                return NotFound();
            }

            var response = new GetGroupResponse
            {
                Group = group
            };

            return Ok(response);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupAsync(string id, Group group)
        {
            await _service.UpdateGroupAsync(id, group);
            return NoContent();
        }

        [HttpPut("leave/{id}")]
        [Authorize(Policy = "GroupMember")]
        public async Task<IActionResult> LeaveGroupAsync(string id)
        {
            await _service.LeaveGroupAsync(id, LoggedInUser);
            return NoContent();
        }

        [HttpPut("join/{id}")]
        public async Task<IActionResult> JoinGroupAsync(string id)
        {
            await _service.AddUserRoleToGroupMemberAsync(id, LoggedInUser, "Member");
            return NoContent();
        }

        [HttpPut("manager/add/{id}")]
        public async Task<IActionResult> AddManagerAsync(string id)
        {
            await _service.AddUserRoleToGroupMemberAsync(id, LoggedInUser, "Manager");
            return NoContent();
        }

        // POST: api/MemberInGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroupAsync(Group group)
        {
            await _service.CreateGroupAsync(group, LoggedInUser);
            return CreatedAtAction("GetGroupsAsync", new { id = group.GroupId }, group);
        }

        // DELETE: api/MemberInGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupAsync(string id)
        {
            await _service.DeleteGroupAsync(id);
            return NoContent();
        }
    }
}