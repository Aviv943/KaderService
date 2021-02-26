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

        public GroupsService(KaderContext context)
        {
            _context = context;
        }

        //Gets
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await _context.Group.ToListAsync();
        }

        //Get
        public async Task<Group> GetGroupAsync(string id)
        {
            return await _context.Group.FindAsync(id);
        }

        //Put/ Update
        public async Task UpdateGroupAsync(string id, Group group)
        {
            if (!id.Equals(group.Id) )
            {
                throw new Exception("Id is not equal to post.Id");
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
            return _context.Group.Any(e => e.Id == id);
        }

        public async Task CreateGroupAsync(Group group)
        {
            _context.Group.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGroupAsync(string id)
        {
            var group = await _context.Group.FindAsync(id);
            if (group == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Group.Remove(group);
            await _context.SaveChangesAsync();
        }

    }
}
