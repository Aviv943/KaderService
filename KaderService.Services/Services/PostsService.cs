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
    public class PostsService
    {
        private readonly KaderContext _context;

        public PostsService(KaderContext context)
        {
            _context = context;
        }

        //Gets
        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
           return await _context.Post.ToListAsync();
        }

        //Get
        public async Task<Post> GetPostAsync(int id)
        {
            return await _context.Post.FindAsync(id);
        }

        //Put/ Update
        public async Task UpdatePostAsync(int id, Post post)
        {
            if (id != post.Id)
            {
                throw new Exception("Id is not equal to post.Id");
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!PostExists(id))
                {
                    throw new KeyNotFoundException();
                }

                throw;
            }
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }

        //Post/ Create
        public async Task CreatePostAsync(Post post)
        {
            await _context.Post.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        //Delete

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                throw new KeyNotFoundException();
            }
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
        }

    }
}
