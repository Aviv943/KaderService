using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using KaderService.Services.ViewModels;

namespace KaderService.Seeder.Seeds
{
    public class Users : Seeds
    {
        public override async Task SeedAsync()
        {
            CreateClient();
            List<RegisterModel> users = GetData<RegisterModel>();

            foreach (RegisterModel user in users)
            {
                try
                {
                    await UsersClient.RegisterAsync(user);
                    Console.WriteLine($"User created '{user.Username}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"User could not be created, ex: '{e.Message}'");
                }
            }
        }

        public virtual List<T> GetData<T>()
        {
            var users = new List<RegisterModel>
            {
                new()
                {
                    Email = "Yossi@gmail.com",
                    Username = "Yossi",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Ami@gmail.com",
                    Username = "Ami",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Boris@gmail.com",
                    Username = "Boris",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Yoram@gmail.com",
                    Username = "Yossi",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Benda@gmail.com",
                    Username = "Benda",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Moshe@gmail.com",
                    Username = "Moshe",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "David@gmail.com",
                    Username = "David",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Beni@gmail.com",
                    Username = "Beni",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Jeff@gmail.com",
                    Username = "Jeff",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Jacob@gmail.com",
                    Username = "Jacob",
                    Password = "Bolila1!"
                },
                new()
                {
                    Email = "Boten@gmail.com",
                    Username = "Boten",
                    Password = "Bolila1!"
                }
            };

            return (List<T>)Convert.ChangeType(users, typeof(List<RegisterModel>));
        }
    }
}