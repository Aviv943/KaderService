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
            try
            {
                Directory.Delete("c:/inetpub/wwwroot/users", true);
            }
            catch (Exception)
            {
                Directory.CreateDirectory("c:/inetpub/wwwroot/users");
            }

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
                    byte[] image = client.DownloadData(new Uri("https://thispersondoesnotexist.com/image"));
                    await using Stream stream1 = new MemoryStream(image);
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
                new() { Email = "Yossi@gmail.com", Username = "Yossi", FirstName = "Yossi", LastName = "Mashmo", PhoneNumber = "+972-559-555-850" },
                new() { Email = "Ami@gmail.com", Username = "Ami", FirstName = "Ami", LastName = "Argazim", PhoneNumber = "+972-505-552-394" },
                new() { Email = "Boris@gmail.com", Username = "Boris", FirstName = "Boris", LastName = "Nagha", PhoneNumber = "+972-585-558-990" },
                new() { Email = "Yoram@gmail.com", Username = "Yoram", FirstName = "Yoram", LastName = "LoGahon", PhoneNumber = "+972-585-556-971" },
                new() { Email = "Benda@gmail.com", Username = "Benda", FirstName = "Guy", LastName = "Ben David", PhoneNumber = "+972-545-557-035" },
                new() { Email = "Moshe@gmail.com", Username = "Moshe", FirstName = "Moshe", LastName = "Avanim", PhoneNumber = "+972-585-554-630" },
                new() { Email = "David@gmail.com", Username = "David", FirstName = "David", LastName = "Sela", PhoneNumber = "+972-505-557-128" },
                new() { Email = "Beni@gmail.com", Username = "Beni", FirstName = "Beni", LastName = "Saranga", PhoneNumber = "+972-505-551-209" },
                new() { Email = "Jeff@gmail.com", Username = "Jeff", FirstName = "Jeff", LastName = "Abutbul", PhoneNumber = "+972-556-555-348" },
                new() { Email = "Jacob@gmail.com", Username = "Jacob", FirstName = "Jacob", LastName = "Uda", PhoneNumber = "+972-552-355-554" },
                new() { Email = "Noam@gmail.com", Username = "Noam", FirstName = "Noam", LastName = "Trump", PhoneNumber = "+972-557-155-505" },
                new() { Email = "Mathe@gmail.com", Username = "Mathe", FirstName = "Mathe", LastName = "Boskila", PhoneNumber = "+972-505-558-108" },
                new() { Email = "Ryan@gmail.com", Username = "Ryan", FirstName = "Ryan", LastName = "Jeff", PhoneNumber = "+972-556-555-016" },
                new() { Email = "Jerry@gmail.com", Username = "Jerry", FirstName = "Jerry", LastName = "Kela", PhoneNumber = "+972-556-555-822" },
                new() { Email = "Linda@gmail.com", Username = "Linda", FirstName = "Linda", LastName = "Ohi", PhoneNumber = "+972-585-556-462" },
                new() { Email = "Martha@gmail.com", Username = "Martha", FirstName = "Martha", LastName = "Bulila", PhoneNumber = "+972-559-555-090" },
                new() { Email = "GaryMAndersen@jourrapide.com", Username = "Thichatherne", FirstName = "Gary", LastName = "Anderson", PhoneNumber = "+972-585-551-916" },
                new() { Email = "JoseDWashington@teleworm.us", Username = "Comeng", FirstName = "Jose", LastName = "Wash", PhoneNumber = "+972-505-551-769" },
                new() { Email = "IreneJSimmons@rhyta.com", Username = "Thowintal", FirstName = "Irene", LastName = "Simon", PhoneNumber = "+972-505-557-512" },
                new() { Email = "JeffreyCCorn@rhyta.com", Username = "Mareceing", FirstName = "Jeff", LastName = "Corn", PhoneNumber = "+972-559-555-464" },
                new() { Email = "MichaelSFackler@armyspy.com", Username = "Whamonothen", FirstName = "Michael", LastName = "Fackler", PhoneNumber = "+972-525-553-883" },
                new() { Email = "JoeLRodriguez@armyspy.com", Username = "Thatence", FirstName = "Joe", LastName = "Rodrigez", PhoneNumber = "+972-585-554-614" },
                new() { Email = "AndyATerry@rhyta.com", Username = "Frapter", FirstName = "Andy", LastName = "Terry", PhoneNumber = "+972-557-155-547" },
                new() { Email = "WendyKSager@armyspy.com", Username = "Gireal1940", FirstName = "Wendy", LastName = "Sager", PhoneNumber = "+972-525-558-294" },
                new() { Email = "JoseSAdams@dayrep.com", Username = "Doesire1971", FirstName = "Jose", LastName = "Adams", PhoneNumber = "+972-552-355-512" },
                new() { Email = "VenusSPeterson@rhyta.com", Username = "Tomer1988", FirstName = "Tomer", LastName = "Peterson", PhoneNumber = "+972-559-555-528" },
            };

            return (List<T>)Convert.ChangeType(users, typeof(List<RegisterModel>));
        }
    }
}