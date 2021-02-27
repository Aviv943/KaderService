using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Services
{
    public class PostsService
    {
        private readonly KaderContext _context;
        private readonly UserManager<User> _userManager;

        public PostsService(KaderContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Gets
        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        //Get
        public async Task<Post> GetPostAsync(string id)
        {
            return await _context.Posts.FindAsync(id);
        }

        //Put/ Update
        public async Task UpdatePostAsync(string id, Post post)
        {
            if (!id.Equals(post.Id))
            {
                throw new Exception("Id is not equal to post.Id");
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    throw new KeyNotFoundException();
                }

                throw;
            }
        }

        private bool PostExists(string id)
        {
            return _context.Posts.Any(e => e.Id.Equals(id));
        }

        //Post/ Create
        public async Task CreatePostAsync(Post post, User user)
        {
            post.Creator = user;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        //Delete

        public async Task DeletePostAsync(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}