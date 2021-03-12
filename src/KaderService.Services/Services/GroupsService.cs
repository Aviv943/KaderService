using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KaderService.Services.Services
{
    public class GroupsService
    {
        private readonly KaderContext _context;
        private readonly PostsService _postsService;

        public GroupsService(KaderContext context, PostsService postsService)
        {
            _context = context;
            _postsService = postsService;
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<Group> GetGroupAsync(string id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<ICollection<Post>> GetGroupPostsByIdAsync(string id)
        {
            var group = await GetGroupAsync(id);

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

        public async Task CreateGroupAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGroupAsync(string id)
        {
            Group group = await _context.Groups.FindAsync(id);

            if (group == null)
            {
                throw new KeyNotFoundException();
            }

            await _postsService.DeletePostsAsync(group.Posts);

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
    }
}