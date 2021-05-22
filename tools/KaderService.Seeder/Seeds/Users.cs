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
                user.Password = "Bolila1!";

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
                new() { Email = "Yossi@gmail.com", Username = "Yossi" },
                new() { Email = "Ami@gmail.com", Username = "Ami" },
                new() { Email = "Boris@gmail.com", Username = "Boris" },
                new() { Email = "Yoram@gmail.com", Username = "Yoram" },
                new() { Email = "Benda@gmail.com", Username = "Benda" },
                new() { Email = "Moshe@gmail.com", Username = "Moshe" },
                new() { Email = "David@gmail.com", Username = "David" },
                new() { Email = "Beni@gmail.com", Username = "Beni" },
                new() { Email = "Jeff@gmail.com", Username = "Jeff" },
                new() { Email = "Jacob@gmail.com", Username = "Jacob" },
                new() { Email = "Noam@gmail.com", Username = "Noam" },
                new() { Email = "Mathe@gmail.com", Username = "Mathe" },
                new() { Email = "Ryan@gmail.com", Username = "Ryan" },
                new() { Email = "Jerry@gmail.com", Username = "Jerry" },
                new() { Email = "Linda@gmail.com", Username = "Linda" },
                new() { Email = "Martha@gmail.com", Username = "Martha" },
                new() { Email = "GaryMAndersen@jourrapide.com", Username = "Thichatherne" },
                new() { Email = "JoseDWashington@teleworm.us", Username = "Comeng" },
                new() { Email = "IreneJSimmons@rhyta.com", Username = "Thowintal" },
                new() { Email = "JeffreyCCorn@rhyta.com", Username = "Mareceing" },
                new() { Email = "MichaelSFackler@armyspy.com", Username = "Whamonothen" },
                new() { Email = "JoeLRodriguez@armyspy.com", Username = "Thatence" },
                new() { Email = "AndyATerry@rhyta.com", Username = "Frapter" },
                new() { Email = "WendyKSager@armyspy.com", Username = "Gireal1940" },
                new() { Email = "JoseSAdams@dayrep.com", Username = "Doesire1971" },
                new() { Email = "VenusSPeterson@rhyta.com", Username = "Tomer1988" }
            };

            return (List<T>)Convert.ChangeType(users, typeof(List<RegisterModel>));
        }
    }
}