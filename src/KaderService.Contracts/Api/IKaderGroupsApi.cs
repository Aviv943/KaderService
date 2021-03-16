using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderGroupsApi
    {
        [Get("/api/groups")]
        Task<IEnumerable<Group>> GetGroupsAsync();

        [Get("/api/groups/{id}")]
        Task<Group> GetGroupAsync(string id);

        [Get("/api/groups/{id}/posts")]
        Task<ICollection<Post>> GetGroupPostsByIdAsync(string id);

        [Put("/api/groups/{id}")]
        Task UpdateGroupAsync(string id, Group group);

        [Post("/api/groups")]
        [Headers("Authorization: Bearer")]
        Task CreateGroupAsync(Group group);

        [Delete("/api/groups/{id}")]
        Task DeleteGroupAsync(string id);
    }
}
