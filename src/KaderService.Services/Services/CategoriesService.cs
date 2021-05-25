using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using KaderService.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaderService.Services.Services
{
    public class CategoriesService
    {
        private readonly KaderContext _context;
        private readonly CategoriesRepository _repository;

        public CategoriesService(KaderContext context, CategoriesRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _repository.GetCategoriesAsync();
        }

        public async Task<string> PostCategoryImageAsync(string name, IFormFile file)
        {
            Category category = await _context.Categories.FindAsync(name);

            if (category == null)
            {
                throw new KeyNotFoundException("Category could not be found");
            }

            var fileName = $"{category.Name}.jpg";
            DirectoryInfo baseUserDirectory = Directory.CreateDirectory("c:/inetpub/wwwroot/categories");
            string filePath = Path.Combine(baseUserDirectory.FullName, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            category.ImageUri = $"/categories/{fileName}";

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return $"http://kader.cs.colman.ac.il{category.ImageUri}";
        }

        public async Task PostCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Category>> GetCategoryAsync(string name)
        {
            return await _repository.GetCategory(name);
        }
    }
}