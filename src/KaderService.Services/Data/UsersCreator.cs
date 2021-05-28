using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using KaderService.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KaderService.Services.Data
{
    public class UsersCreator
    {
        private static DataCreator _dataCreator;

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            await using var context = new KaderContext(serviceProvider.GetRequiredService<DbContextOptions<KaderContext>>());
            _dataCreator = new DataCreator(serviceProvider);

            await Create();
        }

        private static async Task Create()
        {
            Directory.Delete("c:/inetpub/wwwroot/users", true);
            List<RegisterModel> users = GetData<RegisterModel>();

            foreach (RegisterModel registerUser in users)
            {
                registerUser.Password = "Bolila1!";

                User user;

                try
                {
                    user = await _dataCreator.UsersService.RegisterAsync(registerUser);
                    Console.WriteLine($"User created '{registerUser.Username}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"User could not be created, ex: '{e.Message}'");
                    continue;
                }

                try
                {
                    using var client = new WebClient();
                    byte[] a = client.DownloadData(new Uri("https://thispersondoesnotexist.com/image"));
                    await using Stream stream1 = new MemoryStream(a);
                    var file = new FormFile(stream1, 0, stream1.Length, null!, "Image.jpg")
                    {
                        Headers = new HeaderDictionary(),
                    };

                    await _dataCreator.UsersService.UpdateUserImageAsync(user, file);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not upload image to user {user.Id}, ex: '{e.Message}'");
                }
            }
        }

        public static List<T> GetData<T>()
        {
            var users = new List<RegisterModel>
            {
                new() { Email = "Yossi@gmail.com", Username = "Yossi", FirstName = "Yossi", LastName = "Mashmo"},
                new() { Email = "Ami@gmail.com", Username = "Ami", FirstName = "Ami", LastName = "Argazim"},
                new() { Email = "Boris@gmail.com", Username = "Boris", FirstName = "Boris", LastName = "Nagha"},
                new() { Email = "Yoram@gmail.com", Username = "Yoram", FirstName = "Yoram", LastName = "LoGahon"},
                new() { Email = "Benda@gmail.com", Username = "Benda", FirstName = "Guy", LastName = "Ben David"},
                new() { Email = "Moshe@gmail.com", Username = "Moshe", FirstName = "Moshe", LastName = "Avanim"},
                new() { Email = "David@gmail.com", Username = "David", FirstName = "David", LastName = "Sela"},
                new() { Email = "Beni@gmail.com", Username = "Beni", FirstName = "Beni", LastName = "Saranga"},
                new() { Email = "Jeff@gmail.com", Username = "Jeff", FirstName = "Jeff", LastName = "Abutbul"},
                new() { Email = "Jacob@gmail.com", Username = "Jacob", FirstName = "Jacob", LastName = "Uda"},
                new() { Email = "Noam@gmail.com", Username = "Noam", FirstName = "Noam", LastName = "Trump"},
                new() { Email = "Mathe@gmail.com", Username = "Mathe", FirstName = "Mathe", LastName = "Boskila"},
                new() { Email = "Ryan@gmail.com", Username = "Ryan", FirstName = "Ryan", LastName = "Jeff"},
                new() { Email = "Jerry@gmail.com", Username = "Jerry", FirstName = "Jerry", LastName = "Kela"},
                new() { Email = "Linda@gmail.com", Username = "Linda", FirstName = "Linda", LastName = "Ohi"},
                new() { Email = "Martha@gmail.com", Username = "Martha", FirstName = "Martha", LastName = "Bulila"},
                new() { Email = "GaryMAndersen@jourrapide.com", Username = "Thichatherne", FirstName = "Gary", LastName = "Anderson"},
                new() { Email = "JoseDWashington@teleworm.us", Username = "Comeng", FirstName = "Jose", LastName = "Wash"},
                new() { Email = "IreneJSimmons@rhyta.com", Username = "Thowintal", FirstName = "Irene", LastName = "Simon"},
                new() { Email = "JeffreyCCorn@rhyta.com", Username = "Mareceing", FirstName = "Jeff", LastName = "Corn"},
                new() { Email = "MichaelSFackler@armyspy.com", Username = "Whamonothen", FirstName = "Michael", LastName = "Fackler"},
                new() { Email = "JoeLRodriguez@armyspy.com", Username = "Thatence", FirstName = "Joe", LastName = "Rodrigez"},
                new() { Email = "AndyATerry@rhyta.com", Username = "Frapter", FirstName = "Andy", LastName = "Terry"},
                new() { Email = "WendyKSager@armyspy.com", Username = "Gireal1940", FirstName = "Wendy", LastName = "Sager"},
                new() { Email = "JoseSAdams@dayrep.com", Username = "Doesire1971", FirstName = "Jose", LastName = "Adams"},
                new() { Email = "VenusSPeterson@rhyta.com", Username = "Tomer1988", FirstName = "Tomer", LastName = "Peterson"},
            };

            return (List<T>)Convert.ChangeType(users, typeof(List<RegisterModel>));
        }
    }
}