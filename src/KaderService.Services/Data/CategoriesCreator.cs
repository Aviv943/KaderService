using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KaderService.Services.Data
{
    public class CategoriesCreator
    {
        private static DataCreator _dataCreator;

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            _dataCreator = new DataCreator(serviceProvider);
            await Create();
        }

        private static async Task Create()
        {
            #region Create Groups
            await _dataCreator.LoginRandomUserAsync();
            List<Category> categories = GetData<Category>();

            foreach (Category category in categories)
            {
                try
                {
                    await _dataCreator.CategoriesService.PostCategoryAsync(category);
                    Console.WriteLine($"Category created '{category.Name}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Category could not be created, ex: '{e.Message}'");
                }
            }
            #endregion
        }

        public static List<T> GetData<T>()
        {
            var categories = new List<Category>
            {
                new()
                {
                    Name = "Sport",
                    ImageUri = "/categories/Sport.jpg"
                },
                new()
                {
                    Name = "Beauty",
                    ImageUri = "/categories/Beauty.jpg"
                },
                new()
                {
                    Name = "Books",
                    ImageUri = "/categories/Books.jpg"
                },
                new()
                {
                    Name = "Cars",
                    ImageUri = "/categories/Cars.jpg"
                },
                new()
                {
                    Name = "Cooking",
                    ImageUri = "/categories/Cooking.jpg"
                },
                new()
                {
                    Name = "Food",
                    ImageUri = "/categories/Food.jpg"
                },
                new()
                {
                    Name = "Garden",
                    ImageUri = "/categories/Garden.jpg"
                },
                new()
                {
                    Name = "Home",
                    ImageUri = "/categories/Home.jpg"
                },
                new()
                {
                    Name = "Pets",
                    ImageUri = "/categories/Pets.jpg"
                },
                new()
                {
                    Name = "Technology",
                    ImageUri = "/categories/Technology.jpg"
                },
                new()
                {
                    Name = "Tools",
                    ImageUri = "/categories/Tools.jpg"
                },
                new()
                {
                    Name = "Toys",
                    ImageUri = "/categories/Toys.jpg"
                }
            };

            return (List<T>)Convert.ChangeType(categories, typeof(List<Category>));
        }
    }
}