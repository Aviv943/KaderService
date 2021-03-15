﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace KaderService.Services.Services
{
    public class PostsService
    {
        private readonly KaderContext _context;
        private readonly CommentsService _commentsService;

        public PostsService(KaderContext context, CommentsService commentsService)
        {
            _context = context;
            _commentsService = commentsService;
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

        public async Task CreatePostAsync(Post post, User user, string groupId)
        {
            post.Creator = user;
            post.Group = await _context.Groups.FindAsync(groupId);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePostsAsync(Post post, User user, ICollection<string> groupsIds)
        {
            foreach (var groupId in groupsIds)
            {
                await CreatePostAsync(post, user, groupId);
            }
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
            List<Post> posts = await _context.Posts.Where(u => u.Creator.Id == user.Id && u.Group.GroupId == group.GroupId).ToListAsync();

            await DeletePostsAsync(posts);
        }
    }
}