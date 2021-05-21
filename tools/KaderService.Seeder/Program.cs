using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KaderService.Contracts.Api;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;
using Refit;

namespace KaderService.Seeder
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //var baseUrl = "http://kader.cs.colman.ac.il:5000";
            var baseUrl = "http://localhost:5000";
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var seedTypes = new List<string>();

            foreach (string arg in args)
            {
                if (arg.ToLower().StartsWith("-users"))
                {
                    seedTypes.Add("Users");
                }
                else if (arg.ToLower().StartsWith("-categories"))
                {
                    seedTypes.Add("Categories");
                }
                else if (arg.ToLower().StartsWith("-groups"))
                {
                    seedTypes.Add("Groups");
                }
                else if (arg.ToLower().StartsWith("-posts"))
                {
                    seedTypes.Add("Posts");
                }
                else if (arg.ToLower().StartsWith("-comments"))
                {
                    seedTypes.Add("Comments");
                }
                else if (arg.ToLower().StartsWith("-local"))
                {
                    baseUrl = "http://localhost:5000";
                }
            }

            foreach (string seedType in seedTypes)
            {
                Console.WriteLine($"Seeding '{seedType}'");
                var type = Type.GetType($"KaderService.Seeder.Seeds.{seedType}");
                var factory = await Seeds.Seeds.CreateAsync(type, baseUrl);
                await factory.SeedAsync();
                Console.WriteLine("Done");
            }
        }
    }
}