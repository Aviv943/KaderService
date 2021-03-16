using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderPostsApi
    {

        [Get("/api/posts")]
        [Headers("Authorization: Bearer")]
        Task<IEnumerable<Post>> GetPostsAsync();

        [Get("/api/posts/{id}")]
        [Headers("Authorization: Bearer")]
        Task<Post> GetPostAsync(string id);

        [Put("/api/posts/{id}")]
        [Headers("Authorization: Bearer")]
        Task UpdatePostAsync(string id, Post post);

        [Post("/api/posts")]
        [Headers("Authorization: Bearer")]
        Task CreatePostAsync(Post post, User user, string groupId);

        [Delete("/api/posts/{id}")]
        [Headers("Authorization: Bearer")]
        Task DeletePostAsync(string id);
    }
}
