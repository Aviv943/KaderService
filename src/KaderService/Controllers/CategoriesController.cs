using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaderService.Services.Data;
using KaderService.Services.Models;

namespace KaderService.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly KaderContext _context;

        public CategoriesController(KaderContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // PUT: api/Categories/Sports
        [HttpPut("{name}")]
        public async Task<IActionResult> PutCategory(string name, Category category)
        {
            if (name != category.Name)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(name))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.Name))
                {
                    return Conflict();
                }

                throw;
            }

            return Created(string.Empty, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCategory(string name)
        {
            Category category = await _context.Categories.FindAsync(name);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(string name)
        {
            return _context.Categories.Any(e => e.Name == name);
        }
    }
}
