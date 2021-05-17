using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public MyControllerBase(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public User LoggedInUser => !string.IsNullOrWhiteSpace(User?.Identity?.Name) 
            ? _userManager.FindByNameAsync(User?.Identity?.Name).Result 
            : throw new UnauthorizedAccessException("User is not logged in");

        public async Task<User> GetRelevantUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return LoggedInUser;
            }

            User user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User could NOT be found by ID '{userId}'");
            }

            return user;
        }
    }
}