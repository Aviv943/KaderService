using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Repositories
{
    public class GroupsRepository
    {
        private readonly KaderContext _context;

        public GroupsRepository(KaderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetGroupsByUserAsync(User user)
        {
            return await _context.Groups
                .Where(g =>
                    g.Members.Contains(user) ||
                    g.GroupPrivacy == GroupPrivacy.Public ||
                    g.GroupPrivacy == GroupPrivacy.Private)
                .Include(g => g.Posts)
                .Include(g => g.Members)
                .Include(g => g.Managers)
                .Include(g => g.Category)
                .ToListAsync();
        }

        public async Task<Group> GetGroupByGroupIdAsync(string groupId)
        {
            return await _context.Groups
                //.AsNoTrackingWithIdentityResolution()
                .Include(g => g.Members)
                .ThenInclude(u => u.MemberInGroups)
                .Include(g => g.Managers)
                .ThenInclude(u => u.ManagerInGroups)
                .Include(g => g.Posts)
                .ThenInclude(post => post.Creator)
                .Include(g => g.Posts)
                .ThenInclude(post => post.Comments)
                .ThenInclude(comment => comment.Creator)
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);
        }

        public async Task<List<Group>> GetGroupsBySearchAsync(string text, string category)
        {
            IQueryable<Group> query = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                query = _context.Groups
                    .Where(group => group.Name.ToLower().Contains(text))
                    .Include(g => g.Posts)
                    .ThenInclude(post => post.Creator)
                    .Include(g => g.Members)
                    .Include(g => g.Managers)
                    .Include(g => g.Category);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = _context.Groups
                    .Where(group => string.Equals(@group.Category.Name, category, StringComparison.CurrentCultureIgnoreCase))
                    .Include(g => g.Posts)
                    .ThenInclude(post => post.Creator)
                    .Include(g => g.Members)
                    .Include(g => g.Managers)
                    .Include(g => g.Category);
            }

            return query == null ? new List<Group>() : await query.Distinct().ToListAsync();
        }

        public async Task<List<Group>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.Members)
                .Include(g => g.Managers)
                .Include(g => g.Posts)
                .ThenInclude(post => post.Creator)
                .Include(g => g.Category)
                .ToListAsync();
        }
    }
}
