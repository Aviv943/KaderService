﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KaderService.Services.Data
{
    public class AdminsCreator
    {
        private static List<User> _admins;
        private static UserManager<User> _userManager;
        private static RoleManager<IdentityRole> _roleManager;

        public static async Task Initialize(IServiceProvider serviceProvider, string adminPassword)
        {
            await using var context = new KaderContext(serviceProvider.GetRequiredService<DbContextOptions<KaderContext>>());
            _userManager = serviceProvider.GetService<UserManager<User>>();
            _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            await CreateRolesAndUsersAsync(context, adminPassword);
        }

        private static async Task CreateRolesAndUsersAsync(DbContext context, string adminPassword)
        {
            const string admin = "Admin";
            var role = new IdentityRole {Name = admin};
            var roleExists = await _roleManager.RoleExistsAsync(admin);

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
                new()
                {
                    UserName = "Yoni",
                    Email = "yonatan2gross@gmail.com",
                    FirstName = "Yoni",
                    LastName = "Gross",
                    Rating = 1.3,
                    NumberOfRatings = 100,
                },
                new()
                {
                    UserName = "Matan",
                    Email = "matan18061806@gmail.com",
                    FirstName = "Matan",
                    LastName = "Hassin",
                    Rating = 1.9,
                    NumberOfRatings = 1,
                },
                new()
                {
                    UserName = "Aviv",
                    Email = "aviv943@gmail.com",
                    FirstName = "Aviv",
                    LastName = "Miranda",
                    Rating = 4.9,
                    NumberOfRatings = 2000,
                },
                new()
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
    }
}