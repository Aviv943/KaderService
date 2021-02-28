using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KaderService
{
    public class MinimumAgeHandler : AuthorizationHandler<GroupManagerRequirement>
    {
        private readonly KaderContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public MinimumAgeHandler(KaderContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupManagerRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            var groupId = httpContext?.Request?.RouteValues["id"]?.ToString();
            var user = _userManager.FindByNameAsync(context.User.Identity.Name).Result;
            Group group = _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId).Result;

            if (user == null || group == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (group.Managers.Contains(user))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }

    public class GroupManagerRequirement : IAuthorizationRequirement
    {
    }
}
