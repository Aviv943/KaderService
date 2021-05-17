using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderPostsApi
    {
        [Get("/api/posts")]
        Task<IEnumerable<PostView>> GetPostsAsync();

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
