﻿using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderGroupsApi
    {
        [Get("/api/groups/users")]
        Task<IEnumerable<GroupView>> GetGroupsAsync(string userId);

        [Get("/api/groups/{id}")]
        Task<Group> GetGroupAsync(string id);

        [Get("/api/groups/{id}/posts")]
        Task<ICollection<Post>> GetGroupPostsByIdAsync(string id);

        [Put("/api/groups/{id}")]
        Task UpdateGroupAsync(string id, Group group);

        [Put("/api/groups/join/{id}")]
        Task JoinGroupAsync(string id);

        [Post("/api/groups")]
        Task CreateGroupAsync(Group group);

        [Delete("/api/groups/{id}")]
        Task DeleteGroupAsync(string id);
    }
}