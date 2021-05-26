using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Repositories
{
    public class CommentsRepository
    {
        private readonly KaderContext _context;

        public CommentsRepository(KaderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(string postId, PagingParameters pagingParameters)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.Creator)
                .Include(c => c.Post)
                .OrderBy(comment => comment.Created)
                .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentAsync(string id)
        {
            return await _context.Comments
                .Include(c => c.Creator)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
