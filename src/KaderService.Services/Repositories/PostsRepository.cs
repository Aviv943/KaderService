﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.ML.DTO;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Repositories
{
    public class PostsRepository
    {
        private readonly KaderContext _context;

        public PostsRepository(KaderContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsAsync(User user)
        {
            return await _context.Posts
                .Where(post => post.Group.Members.Contains(user))
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .ThenInclude(g => g.Category)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.Creator)
                .ToListAsync();
        }

        public async Task<Post> GetPostAsync(string id)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.Creator)
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .ThenInclude(g => g.Category)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task AddRelatedPostAsync(User user, Post post, RelatedPost relatedPost)
        {
            bool relatedItems = _context.RelatedPosts.Any(rp => rp.PostId == post.PostId && rp.UserId == user.Id);

            if (!relatedItems)
            {
                await _context.RelatedPosts.AddAsync(relatedPost);
                await _context.SaveChangesAsync();
            }
        }

        public bool PostExists(string id)
        {
            return _context.Posts.Any(e => e.PostId.Equals(id));
        }

        public async Task<List<ItemsCustomers>> GetItemsCustomersList(User user)
        {
            return await _context.RelatedPosts
                .Select(post => new ItemsCustomers
                {
                    UserId = user.Id,
                    PostId = post.PostId
                }).ToListAsync();
        }
    }
}
