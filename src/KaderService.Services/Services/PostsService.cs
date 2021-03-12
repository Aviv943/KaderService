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

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostAsync(string id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task UpdatePostAsync(string id, Post post)
        {
            if (!id.Equals(post.PostId))
            {
                throw new Exception("PostId is not equal to post.PostId");
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
            return _context.Posts.Any(e => e.PostId.Equals(id));
        }

        public async Task CreatePostAsync(Post post, User user)
        {
            post.Creator = user;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePostsAsync(Post post, User user, ICollection<string> groupsIds)
        {
            foreach (var groupId in groupsIds)
            {
                post.Group = await _context.Groups.FindAsync(groupId);
                await CreatePostAsync(post, user);
            }
        }

        public async Task DeletePostAsync(string id)
        {
            Post post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}