using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Services
{
    public class CommentsService
    {
        private readonly KaderContext _context;

        public CommentsService(KaderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetCommentAsync(string id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task UpdateCommentAsync(string id, Comment comment)
        {
            if (!id.Equals(comment.Id))
            {
                throw new Exception("PostId is not equal to comment.PostId");
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    throw new KeyNotFoundException();
                }

                throw;
            }
        }
        private bool CommentExists(string id)
        {
            return _context.Comments.Any(e => e.Id.Equals(id));
        }

        public async Task CreateCommentAsync(Comment comment, User user, string postId)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(string id)
        {
            Comment comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentsAsync(ICollection<Comment> comments)
        {
            foreach (Comment comment in comments)
            {
                await DeleteCommentAsync(comment.Id);
            }
        }
    }
}
