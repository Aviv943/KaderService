using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KaderService
{
    public class GroupMemberHandler : AuthorizationHandler<GroupMemberRequirement>
    {
        private readonly KaderContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupMemberHandler(KaderContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupMemberRequirement requirement)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            var groupId = httpContext?.Request.RouteValues["id"]?.ToString();
            string? userName = context.User.Identity.Name;

            if (string.IsNullOrWhiteSpace(userName))
            {
                return Task.CompletedTask;
            }

            User user = _context.Users
                .Include(u => u.MemberInGroups)
                .ThenInclude(g => g.Members)
                .Include(u => u.ManagerInGroups)
                .ThenInclude(u => u.Managers)
                .FirstOrDefaultAsync(u => u.UserName == userName).Result;

            Group group = _context.Groups
                .Include(g => g.Members)
                .ThenInclude(u => u.MemberInGroups)
                .Include(g => g.Managers)
                .ThenInclude(u => u.ManagerInGroups)
                .FirstOrDefaultAsync(g => g.GroupId == groupId).Result;

            if (user == null || group == null)
            {
                return Task.CompletedTask;
            }

            if (!group.Members.Contains(user))
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class GroupMemberRequirement : IAuthorizationRequirement
    {
    }
}
