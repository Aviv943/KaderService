using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KaderService.Services.Data
{
    public class SeedData
    {
        private static List<User> _admins;
        private static UserManager<User> _userManager;
        private static RoleManager<IdentityRole> _roleManager;

        public static async Task Initialize(IServiceProvider serviceProvider, string adminPassword)
        {
            await using var context = new KaderContext(serviceProvider.GetRequiredService<DbContextOptions<KaderContext>>());
            _userManager = serviceProvider.GetService<UserManager<User>>();
            _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            await SeedRolesAndUsersAsync(context, adminPassword);
            await SeedDataBaseAsync(context);
        }

        private static async Task SeedRolesAndUsersAsync(DbContext context, string adminPassword)
        {
            const string admin = "Admin";
            var role = new IdentityRole {Name = admin};
            bool roleExists = await _roleManager.RoleExistsAsync(admin);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(role);
                await CreateAdminsAsync(_userManager, adminPassword);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateAdminsAsync(UserManager<User> userManager, string adminPassword)
        {
            _admins = new List<User>
            {
                new User
                {
                    UserName = "Yoni",
                    Email = "yonatan2gross@gmail.com",
                    FirstName = "Yoni",
                    LastName = "Gross",
                    Rating = 1.3,
                    NumberOfRatings = 100,
                },
                new User
                {
                    UserName = "Matan",
                    Email = "matan18061806@gmail.com",
                    FirstName = "Matan",
                    LastName = "Hassin",
                    Rating = 1.9,
                    NumberOfRatings = 1,
                },
                new User
                {
                    UserName = "Aviv",
                    Email = "aviv943@gmail.com",
                    FirstName = "Aviv",
                    LastName = "Miranda",
                    Rating = 4.9,
                    NumberOfRatings = 2000,
                },
                new User
                {
                    UserName = "Diana",
                    Email = "isakovDiana1@gmail.com",
                    FirstName = "Diana",
                    LastName = "Isakov",
                    Rating = 4.2,
                    NumberOfRatings = 250,
                }
            };

            foreach (User user in _admins)
            {
                IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        private static async Task SeedDataBaseAsync(KaderContext context)
        {
            if (context.Posts.Any() || context.Groups.Any() || context.Comments.Any())
            {
                return;
            }

            await SeedGroupsAsync(context);
            await SeedPostsAsync(context);
            await PopulatePostsInGroups(context);
            //await SeedCommentsAsync(context);
            //SeedOrdersAndPayments(context, items, stores);
        }

        private static async Task SeedCommentsAsync(KaderContext context)
        {
        }

        private static async Task SeedGroupsAsync(KaderContext context)
        {
            var groups = new[]
            {
                new Group()
                {
                    Name = "Area 51",
                    Category = "Sport",
                    Description = "Area 51 Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public,
                    Members = {_admins[1],_admins[2],_admins[3]},
                    Managers = {_admins[1]}
        },
                new Group()
                {
                    Name = "Jokes",
                    Category = "Sport",
                    Description = "Jokes Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private,
                    Members = {_admins[0],_admins[2]},
                    Managers = {_admins[0]}
                },
                new Group()
                {
                    Name = "Students In Colman",
                    Category = "Sport",
                    Description = "Students In Colman Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Members = {_admins[0],_admins[3]},
                    Managers = {_admins[0]}
                },
                new Group()
                {
                    Name = "Cars Pishpeshok",
                    Category = "Sport",
                    Description = "Cars Pishpeshok Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private,
                    Members = {_admins[0],_admins[1],_admins[2],_admins[3]},
                    Managers = {_admins[1],_admins[2]}
                },
                new Group()
                {
                    Name = "Junior Developers Petah-Tikva",
                    Category = "Sport",
                    Description = "Junior Developers Petah-Tikva Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public,
                    Members = {_admins[1],_admins[2]},
                    Managers = {_admins[2]}
                },
                new Group()
                {
                    Name = "Senior Developers Holon",
                    Category = "Sport",
                    Description = "Senior Developers Holon Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private,
                    Members = {_admins[0],_admins[1],_admins[2],_admins[3]},
                    Managers = {_admins[1], _admins[2], _admins[3] }
                },
                new Group()
                {
                    Name = "Budapest for travelers",
                    Category = "Sport",
                    Description = "Budapest for travelers Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public,
                    Members = {_admins[1],_admins[2]},
                    Managers = {_admins[1]}
                },
                new Group()
                {
                    Name = "Prague for travelers",
                    Category = "Sport",
                    Description = "Prague for travelers Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Members = {_admins[0],_admins[1],_admins[2],_admins[3]},
                    Managers = {_admins[2]}
                },
                new Group()
                {
                    Name = "Cheap stuff",
                    Category = "Sport",
                    Description = "Cheap stuff Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private,
                    Members = {_admins[1],_admins[2]},
                    Managers = {_admins[1]}
                },
                new Group()
                {
                    Name = "Bitcoin mining",
                    Category = "Crypto Coin",
                    Description = "Bitcoin mining Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public,
                    Members = {_admins[0],_admins[1],_admins[2],_admins[3]},
                    Managers = {_admins[0], _admins[1], _admins[2] }
                },
            };

            foreach (Group group in groups)
            {
                context.Groups.Add(group);
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedPostsAsync(KaderContext context)
        {
            var posts = new[]
            {
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Technology",
                    Description = "Looking for Technology Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Food",
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Food",
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Harp",
                    Description = "Looking for Harp Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Piano",
                    Description = "Looking for Piano Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Cello",
                    Description = "Looking for Cello Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Bass drum",
                    Description = "Looking for Bass drum Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Bass Guitar",
                    Description = "Looking for Bass Guitar Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Guitar",
                    Description = "Looking for Guitar Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Harmonica",
                    Description = "Looking for Harmonica Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Bell",
                    Description = "Looking for Bell Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Tuba",
                    Description = "Looking for Tuba Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Microphone",
                    Description = "Looking for Microphone Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Mouse",
                    Description = "Looking for Mouse Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "HeadPhones",
                    Description = "Looking for HeadPhones Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "USB Stick",
                    Description = "Looking for USB Stick Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Router",
                    Description = "Looking for Router Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Laptop",
                    Description = "Looking for Laptop Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
                new Post
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Web cam",
                    Description = "Looking for Web cam Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string> {"rak", "sarah"}
                },
            };

          

            await context.SaveChangesAsync();
        }

        private static async Task PopulatePostsInGroups(KaderContext context)
        {
            var posts = context.Posts.AsEnumerable();

            foreach (var post in posts)
            {
                post.Group = await context.Groups.OrderBy(g => Guid.NewGuid()).Take(1).FirstAsync();

            }

            await context.SaveChangesAsync();
        }
    }
}