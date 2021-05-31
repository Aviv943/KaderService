using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Repositories;
using KaderService.Services.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Services
{
    public class CommentsService
    {
        private readonly CommentsRepository _repository;
        private readonly PostsRepository _postsRepository;

        public CommentsService(CommentsRepository repository, PostsRepository postsRepository)
        {
            _repository = repository;
            _postsRepository = postsRepository;
        }

        public async Task<IEnumerable<CommentView>> GetCommentsAsync(string postId, PagingParameters pagingParameters)
        {
            IEnumerable<Comment> commentsForPostAsync = await _repository.GetCommentsAsync(postId, pagingParameters);
            IEnumerable<CommentView> commentViews = commentsForPostAsync.Select(c => new CommentView
            {
                Creator = new UserView
                {
                    UserId = c.Creator.Id,
                    UserName = c.Creator.UserName,
                    FirstName = c.Creator.FirstName,
                    LastName = c.Creator.LastName,
                    Rating = c.Creator.Rating,
                    NumberOfRating = c.Creator.NumberOfRatings,
                    PhoneNumber = c.Creator.PhoneNumber
                },
                PostId = c.PostId,
                CommentId = c.CommentId,
                Content = c.Content,
                Created = c.Created,
            });

            return commentViews;
        }

        public async Task<Comment> GetCommentAsync(string id)
        {
            return await _repository.GetCommentAsync(id);
        }

        public async Task UpdateCommentAsync(string id, Comment comment)
        {
            if (!id.Equals(comment.CommentId))
            {
                throw new Exception("PostNumber is not equal to comment.PostNumber");
            }

            await _repository.UpdateCommentAsync(comment);
        }

        public async Task CreateCommentAsync(Comment comment, User user, string postId)
        {
            Post post = await _postsRepository.GetPostAsync(postId);

            if (post == null)
            {
                throw new Exception("Cannot find post id");
            }

            comment.Post = post;
            comment.Creator = user;
            comment.Created = DateTime.Now;

            await _repository.CreateCommentAsync(comment);
            await _postsRepository.AddRelatedPostAsync(user, post, new RelatedPost(user.UserNumber, post.PostNumber));
        }

        public async Task DeleteCommentAsync(string id)
        {
            Comment comment = await _repository.GetCommentAsync(id);

            if (comment == null)
            {
                throw new KeyNotFoundException();
            }

            await _repository.DeleteCommentAsync(comment);
        }

        public async Task DeleteCommentsAsync(ICollection<Comment> comments)
        {
            foreach (var comment in comments)
            {
                await DeleteCommentAsync(comment.CommentId);
            }
        }
    }
}