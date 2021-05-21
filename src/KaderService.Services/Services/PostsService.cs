using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaderService.ML.DTO;
using KaderService.Services.Data;
using KaderService.Services.Models;
using KaderService.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Services
{
    public class PostsService
    {
        private readonly KaderContext _context;
        private readonly CommentsService _commentsService;
        private readonly CommonService _commonService;
        private readonly PostsRepository _repository;

        public PostsService(KaderContext context, CommentsService commentsService, CommonService commonService, PostsRepository repository)
        {
            _context = context;
            _commentsService = commentsService;
            _commonService = commonService;
            _repository = repository;
        }

        public async Task<List<Post>> GetPostsAsync(User user)
        {
            return await _repository.GetPostsAsync(user);
        }

        public async Task<IEnumerable<Post>> GetRecommendedPostsAsync(User user)
        {
            List<ItemsCustomers> itemsCustomersList = await _repository.GetItemsCustomersList(user);
            List<ItemsCustomers> relatedPostsList = itemsCustomersList;

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
            Post post = await _repository.GetPostAsync(id);

            if (post == null)
            {
                throw new Exception("UpgradeVersion can not be found");
            }

            var relatedPost = new RelatedPost(user.Id, id);
            await _repository.AddRelatedPostAsync(user, post, relatedPost);

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
                if (!_repository.PostExists(id))
                {
                    throw new KeyNotFoundException($"PostId '{post.PostId}' could not be found");
                }

                throw;
            }
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
            foreach (Post post in posts)
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
            Post post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                throw new KeyNotFoundException("Post could not be found");
            }

            const string fileName = "main.jpg";
            var serverFilePath = $"posts/{loggedInUser.Id}/{postId}";
            DirectoryInfo baseUserDirectory = Directory.CreateDirectory($"c:/inetpub/wwwroot/{serverFilePath}");
            string filePath = Path.Combine(baseUserDirectory.FullName, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            post.ImagesUri = new List<string>() { $"/{serverFilePath}/{fileName}" };
            _context.Update(post);
            await _context.SaveChangesAsync();

            return $"http://kader.cs.colman.ac.il/{serverFilePath}/{fileName}";
        }
    }
}