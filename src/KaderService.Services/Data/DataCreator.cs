using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using KaderService.Services.Services;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KaderService.Services.Data
{
    public class DataCreator
    {
        public CategoriesService CategoriesService { get; }
        
        public CommentsService CommentsService { get; }
        
        public CommonService CommonService { get; }
        
        public GroupsService GroupsService { get; }
        
        public PostsService PostsService { get; }
        
        public UsersService UsersService { get; }

        public DataCreator(IServiceProvider serviceProvider)
        {
            using var context = new KaderContext(serviceProvider.GetRequiredService<DbContextOptions<KaderContext>>());
            CategoriesService = serviceProvider.GetService<CategoriesService>();
            CommentsService = serviceProvider.GetService<CommentsService>();
            CommonService = serviceProvider.GetService<CommonService>();
            GroupsService = serviceProvider.GetService<GroupsService>();
            PostsService = serviceProvider.GetService<PostsService>();
            UsersService = serviceProvider.GetService<UsersService>();
        }

        public async Task<User> LoginRandomUserAsync()
        {
            User user = await GetRandomUserAsync(false);
            TokenInfo tokenInfo = await UsersService.LoginAsync(new LoginModel
            {
                Username = user.UserName,
                Password = "Bolila1!"
            });

            Console.WriteLine($"Logged in to userId '{tokenInfo.UserId}'");

            return user;
        }

        public async Task<User> GetRandomUserAsync(bool adminsOnly)
        {
            IEnumerable<User> users;

            if (adminsOnly)
            {
                users = await UsersService.GetAdminsAsync();
            }
            else
            {
                users = await UsersService.GetUsersAsync();
            }

            List<User> usersList = users.ToList();

            if (!usersList.Any())
            {
                throw new NullReferenceException("Users is null");
            }

            var random = new Random();
            int randomValue = random.Next(usersList.Count());

            User user = usersList[randomValue];
            Console.WriteLine($"Got user '{user.UserName}'");

            return user;
        }

        public async Task<GroupView> GetRandomGroupAsync(User user)
        {
            IEnumerable<GroupView> groups = await GroupsService.GetGroupsAsync(user);
            List<GroupView> groupsList = groups.ToList();

            if (!groupsList.Any())
            {
                throw new NullReferenceException("Groups is null");
            }

            var random = new Random();
            int randomValue = random.Next(groupsList.Count());

            GroupView groupView = groupsList[randomValue];
            Console.WriteLine($"Got group '{groupView.Name}'");

            return groupView;
        }

        public async Task<PostView> GetRandomPostAsync(User user)
        {
            List<PostView> posts = await PostsService.GetPostsAsync(null, null);
            List<PostView> postsList = posts.ToList();

            if (!postsList.Any())
            {
                throw new NullReferenceException("Posts is null");
            }

            var random = new Random();
            int randomValue = random.Next(postsList.Count);

            PostView postView = postsList[randomValue];
            await PostsService.GetPostAsync(postView.PostId, user);
            Console.WriteLine($"Got post '{postView.Title}'");

            return postView;
        }

        public async Task<Category> GetRandomCategoryAsync()
        {
            IEnumerable<Category> categories = await CategoriesService.GetCategoriesAsync();
            List<Category> categoriesList = categories.ToList();

            if (!categoriesList.Any())
            {
                throw new NullReferenceException("Categories is null");
            }

            var random = new Random();
            int randomValue = random.Next(categoriesList.Count);

            Category category = categoriesList[randomValue];
            Console.WriteLine($"Got category '{category.Name}'");

            return category;
        }
    }
}
