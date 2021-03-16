using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderUsersApi
    {
        [Post("/api/users/login")]
        Task<TokenInfo> LoginAsync(LoginModel loginModel);

        [Post("/api/users/register")]
        Task<bool> RegisterAsync(RegisterModel registerModel);

        [Get("/api/users")]
        Task<IEnumerable<User>> GetUsersAsync();

        [Get("/api/users/{id}")]
        Task<User> GetUserAsync(string id);

        [Put("/api/users/role/{id}")]
        Task PutRoleAsync(string id, string newRole);

        [Post("/api/users/role/{roleName}")]
        Task PostRoleAsync(string roleName);

        [Delete("/api/users/role/{roleName}")]
        Task DeleteRoleAsync(string roleName);
    }
}