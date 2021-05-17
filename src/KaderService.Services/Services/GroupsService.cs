using KaderService.Services.Data;
using KaderService.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Repositories;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Services.Services
{
    public class GroupsService
    {
        private readonly KaderContext _context;
        private readonly PostsService _postsService;
        private readonly CommonService _commonService;
        private readonly UserManager<User> _userManager;
        private readonly GroupsRepository _repository;

        public GroupsService(KaderContext context, PostsService postsService, CommonService commonService, UserManager<User> userManager, GroupsRepository repository)
        {
            _context = context;
            _postsService = postsService;
            _commonService = commonService;
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IEnumerable<GroupView>> GetGroupsAsync(User user)
        {
            IEnumerable<Group> groups = await _repository.GetGroupsByUserAsync(user);
            IEnumerable<GroupView> groupsViews = groups.Select(group =>
            {
                User userManager = group.Managers.FirstOrDefault(us => us.Id == user.Id);

                return new GroupView
                {
                    Name = group.Name,
                    Category = group.Category,
                    Created = group.Created,
                    Description = group.Description,
                    GroupId = group.GroupId,
                    GroupPrivacy = group.GroupPrivacy,
                    Address = group.Address,
                    ManagersCount = group.Managers.Count,
                    MembersCount = group.Members.Count,
                    PostsCount = group.Posts.Count,
                    IsManager = userManager != null
                };
            });

            return groupsViews;
        }

        public async Task<IEnumerable<GroupView>> SearchGroupsAsync(string text, string category, double? radius, string address)
        {
            List<Group> groups = await _repository.GetGroupsBySearchAsync(text, category);
            List<GroupView> groupViews = groups.Select(ConvertToGroupView).ToList();

            if (!radius.HasValue || string.IsNullOrWhiteSpace(address))
            {
                return groupViews;
            }

            string addressLocation = await _commonService.GetLocationAsync(address);

            foreach (Group group in await _repository.GetAllGroupsAsync())
            {
                string[] gLocation = group.Location.Split(',');
                string[] uLocation = addressLocation.Split(',');
                (double, double) groupLocation = (double.Parse(gLocation[0]), double.Parse(gLocation[1]));
                (double, double) userLocation = (double.Parse(uLocation[0]), double.Parse(uLocation[1]));

                double distance = await _commonService.CalculateDistanceAsync(userLocation, groupLocation);

                if (distance > radius)
                {
                    continue;
                }

                GroupView groupView = ConvertToGroupView(@group);
                groupView.Distance = distance;
                groupViews.Add(groupView);
            }

            return groupViews.OrderBy(view => view.Distance);
        }

        public async Task<Group> GetGroupAsync(string groupId)
        {
            return await _repository.GetGroupByGroupIdAsync(groupId);
        }

        public async Task<GroupView> GetGroupViewAsync(string groupId)
        {
            Group group = await _repository.GetGroupByGroupIdAsync(groupId);

            if (group == null)
            {
                throw new KeyNotFoundException($"Group could not be found by ID '{groupId}'");
            }

            GroupView convertToGroupView = ConvertToGroupView(group);

            return convertToGroupView;
        }

        private static GroupView ConvertToGroupView(Group group)
        {
            var posts = new List<PostView>();

            foreach (Post post in group.Posts)
            {
                List<CommentView> comments = post.Comments.Select(comment => new CommentView
                {
                    Creator = new UserView
                    {
                        UserId = comment.Creator.Id,
                        FirstName = comment.Creator.FirstName,
                        LastName = comment.Creator.LastName,
                        ImageUri = comment.Creator.ImageUri
                    },
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Created = comment.Created
                }).ToList();

                posts.Add(new PostView
                {
                    Creator = new UserView
                    {
                        UserId = post.Creator.Id,
                        FirstName = post.Creator.FirstName,
                        LastName = post.Creator.LastName,
                        ImageUri = post.Creator.ImageUri
                    },
                    PostId = post.PostId,
                    Category = post.Category,
                    Comments = comments,
                    Created = post.Created,
                    Description = post.Description,
                    CommentsCount = comments.Count,
                    ImagesUri = post.ImagesUri,
                    Location = post.Location,
                    Title = post.Title,
                    Type = post.Type
                });
            }

            List<UserView> members = group.Members.Select(member => new UserView
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                ImageUri = member.ImageUri,
                UserId = member.Id,
                Rating = member.Rating,
                NumberOfRating = member.NumberOfRatings
            }).ToList();

            List<UserView> managers = group.Managers.Select(manager => new UserView
            {
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                ImageUri = manager.ImageUri,
                UserId = manager.Id,
                Rating = manager.Rating,
                NumberOfRating = manager.NumberOfRatings
            }).ToList();

            return new GroupView
            {
                Name = group.Name,
                Category = group.Category,
                Created = group.Created,
                Description = group.Description,
                GroupId = group.GroupId,
                GroupPrivacy = group.GroupPrivacy,
                Address = group.Address,
                Managers = managers,
                Members = members,
                PostsCount = group.Posts.Count,
                Posts = posts,
            };
        }

        public async Task<ICollection<Post>> GetGroupPostsByIdAsync(string id)
        {
            var group = await GetGroupAsync(id);
            return group.Posts;
        }

        public async Task UpdateGroupAsync(string id, Group group)
        {
            if (!id.Equals(group.GroupId))
            {
                throw new Exception("PostId is not equal to group.PostId");
            }

            _context.Entry(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    throw new KeyNotFoundException();
                }

                throw;
            }
        }

        private bool GroupExists(string id)
        {
            return _context.Groups.Any(e => e.GroupId.Equals(id));
        }

        public async Task CreateGroupAsync(Group group, User user)
        {
            group.Location = await _commonService.GetLocationAsync(group.Address);

            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();

            await AddUserRoleToGroupMemberAsync(group.GroupId, user, "Manager");
        }

        public async Task DeleteGroupAsync(string id)
        {
            var group = await GetGroupAsync(id);

            if (group == null)
            {
                throw new KeyNotFoundException();
            }

            await _postsService.DeletePostsAsync(group.Posts);
            DeleteAllGroupManagers(group);
            DeleteAllGroupMembers(group);
            _context.Groups.Remove(group);

            await _context.SaveChangesAsync();
        }

        private static void DeleteAllGroupManagers(Group group)
        {
            group.Managers = new List<User>();
        }

        private static void DeleteAllGroupMembers(Group group)
        {
            group.Managers = new List<User>();
        }

        public async Task LeaveGroupAsync(string id, User user)
        {
            var group = await GetGroupAsync(id);

            if (!group.Members.Contains(user))
            {
                throw new AggregateException($"User does not a member in this group ({group.Name})");
            }

            if (group.Managers.Count <= 1)
            {
                await DeleteGroupAsync(id);
                return;
            }

            await _postsService.DeletePostsAsync(user, group);
            await RemoveRoleFromGroupMemberAsync(id, user, "Member");
            await UpdateGroupAsync(id, group);
        }

        public async Task AddUserRoleToGroupMemberAsync(string id, User user, string role)
        {
            var group = await GetGroupAsync(id);

            switch (role)
            {
                case "Member":
                    {
                        group.Members.Add(user);
                        break;
                    }
                case "Manager":
                    {
                        if (!group.Members.Contains(user))
                        {
                            await AddUserRoleToGroupMemberAsync(id, user, "Member");
                        }

                        group.Managers.Add(user);
                        break;
                    }
                default:
                    throw new Exception("Role cannot be found");
            }

            await UpdateGroupAsync(id, group);
        }

        public async Task RemoveRoleFromGroupMemberAsync(string id, User user, string role)
        {
            var group = await GetGroupAsync(id);

            switch (role)
            {
                case "Member":
                    {
                        if (group.Managers.Contains(user))
                        {
                            await RemoveRoleFromGroupMemberAsync(id, user, "Manager");
                        }

                        group.Members.Remove(user);
                        break;
                    }
                case "Manager":
                    {
                        if (!group.Members.Contains(user))
                        {
                            throw new AggregateException("User cannot be added as manager without being a member");
                        }

                        if (group.Managers.Count <= 1)
                        {
                            throw new Exception("User can not be removed from a group when he is the only manager in the group, add another manager or remove the group");
                        }

                        group.Managers.Remove(user);
                        break;
                    }
                default:
                    throw new Exception("Role cannot be found");
            }
        }
    }
}