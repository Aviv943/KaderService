using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Models;
using KaderService.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KaderService.Services.Services
{
    public class CategoriesService
    {
        private readonly CategoriesRepository _repository;

        public CategoriesService(CategoriesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _repository.GetCategoriesAsync();
        }

        public async Task<string> PostCategoryImageAsync(string name, IFormFile file)
        {
            Category category = await _repository.GetCategory(name);

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

            await _repository.UpdateCategoryAsync(category);

            return $"http://kader.cs.colman.ac.il{category.ImageUri}";
        }

        public async Task PostCategoryAsync(Category category)
        {
            await _repository.PostCategoryAsync(category);
        }

        public async Task<Category> GetCategoryAsync(string name)
        {
            return await _repository.GetCategory(name);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _repository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await _repository.DeleteCategoryAsync(category);
        }
    }
}