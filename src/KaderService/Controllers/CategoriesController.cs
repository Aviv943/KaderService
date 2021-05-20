using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaderService.Services.Data;
using KaderService.Services.Models;
using KaderService.Services.Services;

namespace KaderService.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly KaderContext _context;
        private readonly CategoriesService _service;

        public CategoriesController(KaderContext context, CategoriesService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Category>> GetCategoryAsync(string name)
        {
            return await _service.GetCategoryAsync(name);
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

        // POST: api/categories
        [HttpPost("{name}/image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Category>> PostCategoryImageAsync(string name)
        {
            IFormFile file = Request.Form.Files.First();
            var categoryImageUri = await _service.PostCategoryImageAsync(name, file);
            
            return Created(string.Empty, string.Empty);
        }

        // POST: api/categories
        [HttpPost("{name}")]
        public async Task<ActionResult<Category>> PostCategoryAsync(Category category)
        {
            await _service.PostCategoryAsync(category);
            return CreatedAtAction("GetCategoryAsync", new {category.Name}, category);
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