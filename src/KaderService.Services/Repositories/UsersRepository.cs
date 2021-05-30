using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Repositories
{
    public class UsersRepository
    {
        private readonly KaderContext _context;

        public UsersRepository(KaderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                //.Include(u => u.MemberInGroups)
                //.ThenInclude(g => g.Members)
                //.Include(u => u.ManagerInGroups)
                //.ThenInclude(u => u.Managers)
                .ToListAsync();
        }

        public async Task<User> GetUserAsync(string id)
        {
            return await _context.Users
                .Include(u => u.MemberInGroups)
                //.ThenInclude(g => g.Members)
                .Include(u => u.ManagerInGroups)
                //.ThenInclude(u => u.Managers)
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
