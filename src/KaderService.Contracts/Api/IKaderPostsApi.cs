using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderPostsApi
    {
        [Get("/api/posts")]
        Task<IEnumerable<Post>> GetPostsAsync();

        [Get("/api/posts/{userId}")]
        Task<IEnumerable<Post>> GetPostForUserAsync(string userId);

        [Get("/api/posts/{id}")]
        Task<Post> GetPostAsync(string id);

        [Put("/api/posts/{id}")]
        Task UpdatePostAsync(string id, Post post);

        [Post("/api/posts/post/{groupId}")]
        Task CreatePostAsync(Post post, string groupId);

        [Delete("/api/posts/{id}")]
        Task DeletePostAsync(string id);
    }
}
