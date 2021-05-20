using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;

namespace KaderService.Seeder.Seeds
{
    public class Categories : Seeds
    {
        public override async Task SeedAsync()
        {
            await LoginAsync();
            List<Category> categories = GetData<Category>();

            foreach (Category category in categories)
            {
                try
                {
                    await CategoriesClient.PostCategory(category);
                    Console.WriteLine($"Category created '{category.Name}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Category could not be created, ex: '{e.Message}'");
                }
            }
        }

        public virtual List<T> GetData<T>()
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