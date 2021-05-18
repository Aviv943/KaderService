using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaderService.ML.DTO;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Services
{
    public class PostsService
    {
        private readonly KaderContext _context;
        private readonly CommentsService _commentsService;
        private readonly CommonService _commonService;
        private readonly UserManager<User> _userManager;

        public PostsService(KaderContext context, CommentsService commentsService, CommonService commonService, UserManager<User> userManager)
        {
            _context = context;
            _commentsService = commentsService;
            _commonService = commonService;
            _userManager = userManager;
        }

        public async Task<List<Post>> GetPostsAsync(User user)
        {
            return await _context.Posts
                .Where(post => post.Group.Members.Contains(user))
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .Include(p => p.Comments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetRecommendedPostsAsync(User user)
        {
            List<ItemsCustomers> relatedPostsList = _context.RelatedPosts.Select(post => new ItemsCustomers() { UserId = user.Id, PostId = post.PostId }).ToList();

            var mlRequest = new Request
            {
                RelatedPostsList = relatedPostsList,
                UserId = user.Id,
                PostsIds = _context.Posts.Select(post => post.PostId).Distinct().ToList(),
            };

            try
            {
                Dictionary<string, double> postsScore = await ML.Core.Run(mlRequest);
                IEnumerable<KeyValuePair<string, double>> topPosts = postsScore.OrderByDescending(pair => pair.Value).Take(6);
                IQueryable<Post> posts = _context.Posts.Take(int.MaxValue);
                return (from keyValuePair in topPosts from post in posts where keyValuePair.Key == post.PostId select post).ToList();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public async Task<Post> GetPostAsync(string id, User user)
        {
            Post post = await _context.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.Creator)
                .Include(p => p.Creator)
                .Include(p => p.Group)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                throw new Exception("UpgradeVersion can not be found");
            }

            var relatedPost = new RelatedPost(user.Id, id);
            bool relatedItems = _context.RelatedPosts.Any(rp => rp.PostId == post.PostId && rp.UserId == user.Id);

            if (!relatedItems)
            {
                await _context.RelatedPosts.AddAsync(relatedPost);
                await _context.SaveChangesAsync();
            }

            return post;
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

        public async Task CreatePostAsync(Post post, User user, string groupId)
        {
            post.Creator = user;
            post.Group = await _context.Groups.FindAsync(groupId);
            post.Location = await _commonService.GetLocationAsync(post.Address);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(string postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                throw new KeyNotFoundException();
            }

            await _commentsService.DeleteCommentsAsync(post.Comments);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostsAsync(ICollection<Post> posts)
        {
            foreach (var post in posts)
            {
                await DeletePostAsync(post.PostId);
            }
        }

        public async Task DeletePostsAsync(User user, Group group)
        {
            List<Post> posts = await _context.Posts
                .Where(u => u.Creator.Id == user.Id && u.Group.GroupId == group.GroupId)
                .ToListAsync();

            await DeletePostsAsync(posts);
        }

        public async Task<string> CreatePostImageAsync(string postId, User loggedInUser, IFormFile file)
        {
            const string fileName = "main.jpg";
            var serverFilePath = $"{loggedInUser.Id}/{postId}";
            DirectoryInfo baseUserDirectory = Directory.CreateDirectory($"c:/inetpub/wwwroot/{loggedInUser.Id}/{postId}");
            string filePath = Path.Combine(baseUserDirectory.FullName, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            Post post = await _context.Posts.FindAsync(postId);
            post.ImagesUri = new List<string>() { $"/{serverFilePath}/{fileName}" };
            _context.Update(post);
            await _context.SaveChangesAsync();

            return $"http://kader.cs.colman.ac.il/{serverFilePath}/{fileName}";
        }
    }
}