using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KaderService
{
    public class MinimumAgeHandler : AuthorizationHandler<GroupManagerRequirement>
    {
        private readonly KaderContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MinimumAgeHandler(KaderContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupManagerRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            var groupId = httpContext.Request.RouteValues["id"].ToString();
            User user = await _context.Users.FindAsync(context.User.Identity.Name);
            Group group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

            if (user == null && group == null)
            {
                context.Fail();
                return;
            }

            if (group.Managers.Contains(user))
            {
                context.Succeed(requirement);
            }
        }
    }

    public class GroupManagerRequirement : IAuthorizationRequirement
    {
    }
}
