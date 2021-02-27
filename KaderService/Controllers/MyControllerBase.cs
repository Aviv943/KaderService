using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public User LoggedInUser => _userManager.FindByNameAsync(User?.Identity?.Name).Result;
    }
}
