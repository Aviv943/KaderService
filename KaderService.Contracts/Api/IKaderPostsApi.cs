using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Refit;

namespace KaderService.Contracts.Api
{
    interface IKaderPostsApi
    {

        [Get("/api/posts")]
        Task<IEnumerable<Post>> GetPostsAsync();

        [Get("/api/posts/{id}")]
        Task<Post> GetPostAsync(string id);

        [Put("/api/posts/{id}")]
        Task UpdatePostAsync(string id, Post post);

        [Post("/api/posts")]
        Task CreatePostAsync(Post post);

        [Delete("/api/posts/{id}")]
        Task DeletePostAsync(string id);
    }
}
