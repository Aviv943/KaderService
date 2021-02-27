using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Models;
using Refit;

namespace KaderService.Contracts.Api
{
    interface IKaderCommentsApi
    {

        [Get("/api/comments")]
        Task<IEnumerable<Comment>> GetCommentsAsync();

        [Get("/api/comments/{id}")]
        Task<Post> GetCommentAsync(string id);

        [Put("/api/comments/{id}")]
        Task UpdateCommentAsync(string id, Comment comment);

        [Post("/api/comments")]
        Task CreateCommentAsync(Comment comment);

        [Delete("/api/comments/{id}")]
        Task DeleteCommentAsync(string id);
    }
}
