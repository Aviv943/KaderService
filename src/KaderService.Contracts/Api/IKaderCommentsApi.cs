using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderCommentsApi
    {

        [Get("/api/comments")]
        Task<IEnumerable<Comment>> GetCommentsAsync();

        [Get("/api/comments/{id}")]
        Task<Post> GetCommentAsync(string id);

        [Put("/api/comments/{id}")]
        Task UpdateCommentAsync(string id, Comment comment);

        [Post("/api/comments/{postId}")]
        Task CreateCommentAsync(Comment comment, string postId);

        [Delete("/api/comments/{id}")]
        Task DeleteCommentAsync(string id);
    }
}
