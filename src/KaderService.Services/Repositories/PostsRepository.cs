using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.ML.DTO;
using KaderService.Services.Constants;
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

        public async Task<List<Post>> GetPostsAsync(User user, PagingParameters pagingParameters)
        {
            return await _context.Posts
                .Where(post => post.Creator == user)
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .ThenInclude(g => g.Category)
                .Include(p => p.Comments.OrderBy(c => c.Created))
                .ThenInclude(c => c.Creator)
                .OrderByDescending(p => p.Created)
                .Skip((pagingParameters.PageNumber -1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .ToListAsync();
        }

        public async Task<Post> GetPostAsync(string id)
        {
            return await _context.Posts
                .Include(p => p.Comments.OrderBy(c => c.Created))
                .ThenInclude(c => c.Creator)
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .ThenInclude(g => g.Category)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task AddRelatedPostAsync(User user, Post post, RelatedPost relatedPost)
        {
            bool relatedItems = _context.RelatedPosts.Any(rp => rp.PostNumber == post.PostNumber && rp.UserNumber == user.UserNumber);

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
                    UserNumber = user.UserNumber,
                    PostNumber = post.PostNumber
                }).ToListAsync();
        }

        public IQueryable<Post> GetAllPosts()
        {
            return _context.Posts
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .ThenInclude(g => g.Category)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Creator)
                .OrderByDescending(p => p.Created);
        }
    }
}
