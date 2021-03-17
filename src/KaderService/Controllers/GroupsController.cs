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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsAsync()
        {
            return Ok(await _service.GetGroupsAsync());
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<GetPostsResponse>>> GetGroupsForUserAsync(string userId)
        {
            IEnumerable<Group> groupsForUserAsync = await _service.GetGroupsForUserAsync(userId);
            IEnumerable<GroupView> groupViews = groupsForUserAsync.Select(group => new GroupView
            {
                Name = group.Name,
                Category = group.Category,
                Created = group.Created,
                Description = group.Description,
                GroupId = group.GroupId,
                GroupPrivacy = group.GroupPrivacy,
                MainLocation = group.MainLocation,
                ManagersCount = group.Managers.Count,
                MembersCount = group.Members.Count,
                PostsCount = group.Posts.Count
            });

            return Ok(new GetPostsResponse
            {
                GroupView = groupViews.ToList()
            });
        }

        [HttpGet("{id}")]
        //[Authorize(Policy = "GroupManager")]
        public async Task<ActionResult<Group>> GetGroupAsync(string id)
        {
            Group group = await _service.GetGroupAsync(id);

            if (group == null)
            {
                return NotFound();
            }


            return Ok(group);
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