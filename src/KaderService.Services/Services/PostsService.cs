using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaderService.ML;
using KaderService.ML.DTO;
using KaderService.Services.Constants;
using KaderService.Services.Data;
using KaderService.Services.Models;
using KaderService.Services.Repositories;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task<List<PostView>> GetPostsAsync(User user, PagingParameters paging)
        {
            List<Post> posts = await _repository.GetPostsAsync(user, paging);
            List<PostView> postsAsync = ConvertToPostViews(posts);

            return postsAsync;
        }

        private static List<PostView> ConvertToPostViews(IEnumerable<Post> posts)
        {
            List<PostView> postsAsync = posts.Select(p => new PostView
            {
                Creator = new UserView
                {
                    UserId = p.Creator.Id,
                    UserName = p.Creator.UserName,
                    FirstName = p.Creator.FirstName,
                    LastName = p.Creator.LastName,
                    Rating = p.Creator.Rating,
                    NumberOfRating = p.Creator.NumberOfRatings,
                    ImageUri = p.Creator.ImageUri,
                    PhoneNumber = p.Creator.PhoneNumber
                },
                Address = p.Address,
                Created = p.Created,
                GroupId = p.GroupId,
                GroupName = p.Group.Name,
                Category = p.Group.Category,
                Type = p.Type,
                PostId = p.PostId,
                Title = p.Title,
                IsActive = p.IsActive,
                Description = p.Description,
                ImagesUri = p.ImagesUri,
                CommentsCount = p.Comments.Count,
                Comments = new List<CommentView>(p.Comments.Select(comment => new CommentView
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Created = comment.Created,
                    Creator = new UserView
                    {
                        UserId = comment.Creator.Id,
                        FirstName = comment.Creator.FirstName,
                        LastName = comment.Creator.LastName,
                        ImageUri = comment.Creator.ImageUri,
                        PhoneNumber = comment.Creator.PhoneNumber
                    }
                }))
            }).ToList();

            return postsAsync;
        }

        public async Task<IEnumerable<PostView>> GetRecommendedPostsAsync(User user)
        {
            List<ItemsCustomers> itemsCustomersList = await _repository.GetItemsCustomersList(user);

            var mlRequest = new Request
            {
                RelatedPostsList = itemsCustomersList,
                UserNumbers = user.UserNumber,
                PostsNumbers = _context.Posts.Select(post => post.PostNumber).ToList(),
            };

            try
            {
                Dictionary<int, double> postsScore = await Core.Run(mlRequest);
                IEnumerable<KeyValuePair<int, double>> topPosts = postsScore.OrderByDescending(pair => pair.Value).Take(6);
                IQueryable<Post> posts = _repository.GetAllPosts();
                List<Post> recommendedPost = (from keyValuePair in topPosts from post in posts where keyValuePair.Key == post.PostNumber select post).ToList();
                
                return ConvertToPostViews(recommendedPost);
            }
            catch
            {
                return new List<PostView>();
            }
        }

        public async Task<Post> GetPostAsync(string id, User user)
        {
            Post post = await _repository.GetPostAsync(id);

            if (post == null)
            {
                throw new Exception($"PostID '{id}' can not be found");
            }

            var relatedPost = new RelatedPost(user.UserNumber, post.PostNumber);
            await _repository.AddRelatedPostAsync(user, post, relatedPost);

            return post;
        }

        public async Task UpdatePostAsync(string id, Post post)
        {
            if (!id.Equals(post.PostId))
            {
                throw new Exception("PostNumber is not equal to post.PostNumber");
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
                    throw new KeyNotFoundException($"PostNumber '{post.PostId}' could not be found");
                }

                throw;
            }
        }

        public async Task<string> CreatePostAsync(Post post, User user, string groupId)
        {
            post.Group = await _context.Groups.FindAsync(groupId);

            if (post.Group == null)
            {
                throw new Exception($"GroupID '{groupId}' could NOT be found");
            }

            post.IsActive = true;
            post.Creator = user;
            post.Location = await _commonService.GetLocationAsync(post.Address);

            EntityEntry<Post> entry =await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            await _repository.AddRelatedPostAsync(user, post, new RelatedPost(user.UserNumber, post.PostNumber));

            return entry.Entity.PostId;
        }

        public async Task DeletePostAsync(string postId)
        {
            Post post = await _context.Posts.FindAsync(postId);

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

            post.ImagesUri = new List<string> { $"/{serverFilePath}/{fileName}" };
            _context.Update(post);
            await _context.SaveChangesAsync();

            return $"http://kader.cs.colman.ac.il/{serverFilePath}/{fileName}";
        }
    }
}