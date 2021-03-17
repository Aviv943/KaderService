using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Services.Services
{
    public class GroupsService
    {
        private readonly KaderContext _context;
        private readonly PostsService _postsService;
        private readonly UserManager<User> _userManager;

        public GroupsService(KaderContext context, PostsService postsService, UserManager<User> userManager)
        {
            _context = context;
            _postsService = postsService;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Group>> GetGroupsForUserAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            return await _context.Groups
                .Where(g =>
                    g.Members.Contains(user) ||
                    g.GroupPrivacy == GroupPrivacy.Public ||
                    g.GroupPrivacy == GroupPrivacy.Private)
                .Include(g => g.Posts)
                .Include(g => g.Members)
                .Include(g => g.Managers)
                .ToListAsync();

        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> GetGroupAsync(string id)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(u => u.MemberInGroups)
                .Include(g => g.Managers)
                .ThenInclude(u => u.ManagerInGroups)
                .Include(g => g.Posts)
                .ThenInclude(post => post.Group)
                .FirstOrDefaultAsync(g => g.GroupId == id);
        }

        public async Task<ICollection<Post>> GetGroupPostsByIdAsync(string id)
        {
            Group group = await GetGroupAsync(id);
            return group.Posts;
        }

        public async Task UpdateGroupAsync(string id, Group group)
        {
            if (!id.Equals(group.GroupId))
            {
                throw new Exception("PostId is not equal to group.PostId");
            }

            _context.Entry(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    throw new KeyNotFoundException();
                }

                throw;
            }
        }

        private bool GroupExists(string id)
        {
            return _context.Groups.Any(e => e.GroupId.Equals(id));
        }

        public async Task CreateGroupAsync(Group group, User user)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            await AddUserRoleToGroupMemberAsync(group.GroupId, user, "Manager");
        }

        public async Task DeleteGroupAsync(string id)
        {
            Group group = await GetGroupAsync(id);

            if (group == null)
            {
                throw new KeyNotFoundException();
            }

            await _postsService.DeletePostsAsync(group.Posts);
            DeleteAllGroupManagers(group);
            DeleteAllGroupMembers(group);
            _context.Groups.Remove(group);

            await _context.SaveChangesAsync();
        }

        private static void DeleteAllGroupManagers(Group group)
        {
            group.Managers = new List<User>();
        }

        private static void DeleteAllGroupMembers(Group group)
        {
            group.Managers = new List<User>();
        }

        public async Task LeaveGroupAsync(string id, User user)
        {
            Group group = await GetGroupAsync(id);

            if (!group.Members.Contains(user))
            {
                throw new AggregateException($"User does not a member in this group ({group.Name})");
            }

            if (group.Managers.Count <= 1)
            {
                await DeleteGroupAsync(id);
                return;
            }

            await _postsService.DeletePostsAsync(user, group);
            await RemoveRoleFromGroupMemberAsync(id, user, "Member");
            await UpdateGroupAsync(id, group);
        }

        public async Task AddUserRoleToGroupMemberAsync(string id, User user, string role)
        {
            Group group = await GetGroupAsync(id);

            switch (role)
            {
                case "Member":
                    {
                        group.Members.Add(user);
                        break;
                    }
                case "Manager":
                    {
                        if (!group.Members.Contains(user))
                        {
                            await AddUserRoleToGroupMemberAsync(id, user, "Member");
                        }

                        group.Managers.Add(user);
                        break;
                    }
                default:
                    throw new Exception("Role cannot be found");
            }

            await UpdateGroupAsync(id, group);
        }

        public async Task RemoveRoleFromGroupMemberAsync(string id, User user, string role)
        {
            Group group = await GetGroupAsync(id);

            switch (role)
            {
                case "Member":
                    {
                        if (group.Managers.Contains(user))
                        {
                            await RemoveRoleFromGroupMemberAsync(id, user, "Manager");
                        }

                        group.Members.Remove(user);
                        break;
                    }
                case "Manager":
                    {
                        if (!group.Members.Contains(user))
                        {
                            throw new AggregateException("User cannot be added as manager without being a member");
                        }

                        if (group.Managers.Count <= 1)
                        {
                            throw new Exception("User can not be removed from a group when he is the only manager in the group, add another manager or remove the group");
                        }

                        group.Managers.Remove(user);
                        break;
                    }
                default:
                    throw new Exception("Role cannot be found");
            }
        }
    }
}