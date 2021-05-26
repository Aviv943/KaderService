using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Data;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KaderService.Services.Services
{
    public class UsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly KaderContext _context;
        private readonly IConfiguration _configuration;

        public UsersService(IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, KaderContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenInfo> LoginAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                throw new UnauthorizedAccessException();
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new ("userid", user.Id),
                new ("email", user.Email),
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new TokenInfo
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Expiration = token.ValidTo,
            };
        }

        public async Task<bool> RegisterAsync(RegisterModel registerModel)
        {
            var userExists = await _userManager.FindByNameAsync(registerModel.Username);

            if (userExists != null)
            {
                throw new ArgumentException("User already exists!");
            }

            var user = new User
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username,
                Created = DateTime.Now,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);

            return result.Succeeded;
        }


        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.MemberInGroups)
                .ThenInclude(g => g.Members)
                .Include(u => u.ManagerInGroups)
                .ThenInclude(u => u.Managers).ToListAsync();
        }

        public async Task<IList<User>> GetAdminsAsync()
        {
            return await _userManager.GetUsersInRoleAsync("Admin");
        }

        private async Task<User> GetUserAsync(string id)
        {
            return await _context.Users
                .Include(u => u.MemberInGroups)
                //.ThenInclude(g => g.Members)
                .Include(u => u.ManagerInGroups)
                //.ThenInclude(u => u.Managers)
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserView> GetUserViewAsync(string userId)
        {
            User user = await GetUserAsync(userId);

            if (user == null)
            {
                throw new Exception($"User is not exist by id '{userId}'");
            }

            //List<GroupView> managersGroupViews = user.ManagerInGroups.Select(group => new GroupView
            //{
            //    Name = group.Name,
            //    Category = group.Category,
            //    Created = group.Created,
            //    Description = group.Description,
            //    GroupId = group.GroupId,
            //    GroupPrivacy = group.GroupPrivacy,
            //    Address = group.Address,
            //    ManagersCount = group.Managers.Count,
            //    MembersCount = group.Members.Count,
            //    PostsCount = group.Posts.Count
            //}).ToList();

            //List<GroupView> membersGroupViews = user.MemberInGroups.Select(group => new GroupView
            //{
            //    Name = group.Name,
            //    Category = group.Category,
            //    Created = group.Created,
            //    Description = group.Description,
            //    GroupId = group.GroupId,
            //    GroupPrivacy = group.GroupPrivacy,
            //    Address = group.Address,
            //    ManagersCount = group.Managers.Count,
            //    MembersCount = group.Members.Count,
            //    PostsCount = group.Posts.Count
            //}).ToList();

            return new UserView
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUri = user.ImageUri,
                NumberOfRating = user.NumberOfRatings,
                Rating = user.Rating,
                UserId = userId,
                MemberInGroupsCount = user.MemberInGroups.Count,
                ManagerInGroupsCount = user.ManagerInGroups.Count,
                PostsCount = user.Posts.Count
            };
        }

        public async Task<IdentityRole> GetRoleAsync(string roleName)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(roleName);
            
            if (role == null)
            {
                throw new Exception($"The {roleName} was not found");
            }
            
            return role;
        }

        public async Task PutRoleAsync(string id, string newRole)
        {
            User user = await GetUserAsync(id);
            await _userManager.AddToRoleAsync(user, newRole);
        }

        public async Task PostRoleAsync(string roleName)
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            try
            {
                IdentityRole role = await GetRoleAsync(roleName);
                await _roleManager.DeleteAsync(role);
            }
            catch (Exception e)
            {
                Console.WriteLine($"The {roleName} failed to delete, error: {e}");
            }
        }

        public async Task<string> UpdateUserImageAsync(User user, IFormFile file)
        {
            const string fileName = "profile.jpg";
            var serverFilePath = $"users/{user.Id}";
            DirectoryInfo baseUserDirectory = Directory.CreateDirectory($"c:/inetpub/wwwroot/{serverFilePath}");
            string filePath = Path.Combine(baseUserDirectory.FullName, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            user.ImageUri = $"/{serverFilePath}/{fileName}";
            await _userManager.UpdateAsync(user);
            _context.Entry(user).State = EntityState.Modified;

            return $"http://kader.cs.colman.ac.il/{serverFilePath}/{fileName}";
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
            //_context.Entry(user).State = EntityState.Modified;
        }

        public async Task AddRatingAsync(string userId, double newRating)
        {
            User user = await _userManager.FindByIdAsync(userId);
            user.Rating = (user.NumberOfRatings * user.Rating + newRating) / ++user.NumberOfRatings;
            await _userManager.UpdateAsync(user);
        }
    }
}