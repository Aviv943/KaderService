using System;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KaderService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<KaderContext>();
                await context.Database.MigrateAsync();

                var config = host.Services.GetRequiredService<IConfiguration>();

                var userManager = services.GetService<UserManager<User>>();
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                bool roleExists = await roleManager.RoleExistsAsync("Admin");

                if (!roleExists)
                {
                    await AdminsCreator.Initialize(services, config["AdminKey"], userManager, roleManager);
                }

                if (await context.Categories.CountAsync() == 0)
                {
                    await CategoriesCreator.Initialize(services);
                }

                if (await context.Users.CountAsync() == 4)
                {
                    await UsersCreator.Initialize(services);
                }

                if (await context.Groups.CountAsync() == 0)
                {
                    await GroupsCreator.Initialize(services);
                }

                if (await context.Posts.CountAsync() == 0)
                {
                    await PostsCreator.Initialize(services, context);
                }

                if (await context.Comments.CountAsync() == 0)
                {
                    await CommentsCreator.Initialize(services);
                }


            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error occurred seeding the DB.");
            }


            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://0.0.0.0:5000", "http://::5000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
