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
    public class CategoriesRepository
    {
        private readonly KaderContext _context;

        public CategoriesRepository(KaderContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategory(string name)
        {
            return await _context.Categories.FindAsync(name);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
