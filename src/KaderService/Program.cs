using System;
using KaderService.Services.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KaderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<KaderContext>();
                context.Database.Migrate();

                var config = host.Services.GetRequiredService<IConfiguration>();
                AdminsCreator.Initialize(services, config["AdminKey"]).Wait();
            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error occurred seeding the DB.");
            }

            host.Run();
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
