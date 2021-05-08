using System;
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
    }
}