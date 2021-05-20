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
                    Name = "Sport"
                },
                new()
                {
                    Name = "Crypto"
                },
                new()
                {
                    Name = "Technology"
                },
                new()
                {
                    Name = "Food"
                },
                new()
                {
                    Name = "Music"
                }
            };

            return (List<T>)Convert.ChangeType(categories, typeof(List<Category>));
        }
    }
}