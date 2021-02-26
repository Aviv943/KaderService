using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //Gets

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comment.ToListAsync();
        }

        //Get

        public async Task<Comment> GetCommentAsync(string id)
        {
            return await _context.Comment.FindAsync(id);
        }

        //Put/ Update
        public async Task UpdateCommentAsync(string id, Comment comment)
        {
            if (id.Equals(comment.Id))
            {
                throw new Exception("Id is not equal to comment.Id");
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
            return _context.Comment.Any(e => e.Id.Equals(id));
        }

        //Post/ Create
        public async Task CreateCommentAsync(Comment comment)
        {
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(string id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
        }


    }
}
