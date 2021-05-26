using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaderService.Services.Models;
using KaderService.Services.Services;

namespace KaderService.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _service;

        public CategoriesController(CategoriesService service)
        {
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
            return Ok(await _service.GetCategoriesAsync());
        }

        // PUT: api/Categories/Sports
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateCategory(string name, Category category)
        {
            if (name != category.Name)
            {
                return BadRequest();
            }

            await _service.UpdateCategoryAsync(category);

            return NoContent();
        }

        // POST: api/categories
        [HttpPost("{name}/image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Category>> PostCategoryImageAsync(string name)
        {
            IFormFile file = Request.Form.Files.First();
            string categoryImageUri = await _service.PostCategoryImageAsync(name, file);
            
            return Created(string.Empty, categoryImageUri);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategoryAsync(Category category)
        {
            await _service.PostCategoryAsync(category);
            return CreatedAtAction("GetCategoryAsync", new {category.Name}, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCategory(string name)
        {
            Category category = await _service.GetCategoryAsync(name);

            if (category == null)
            {
                return NotFound();
            }

            await _service.DeleteCategoryAsync(category);

            return NoContent();
        }
    }
}