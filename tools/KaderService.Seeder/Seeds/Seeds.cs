using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using KaderService.Contracts.Api;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using KaderService.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace KaderService.Seeder.Seeds
{
    public abstract class Seeds
    {
        public static IKaderUsersApi UsersClient;
        public static IKaderGroupsApi GroupsClient;
        public static IKaderPostsApi PostsClient;
        public static IKaderCommentsApi CommentsClient;
        public static IKaderCategoriesApi CategoriesClient;
        public static string BaseUrl;

        public static async Task<Seeds> CreateAsync(Type type, string baseUrl)
        {
            BaseUrl = baseUrl;
            return (Seeds)Activator.CreateInstance(type);
        }

        public static async Task<TokenInfo> LoginAsync()
        {
            HttpClient client = CreateClient();
            User user = await GetRandomUserAsync(true);

            TokenInfo tokenInfo = await UsersClient.LoginAsync(new LoginModel
            {
                Username = user.UserName,
                Password = "Bolila1!"
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenInfo.Token);

            GroupsClient = RestService.For<IKaderGroupsApi>(client);
            PostsClient = RestService.For<IKaderPostsApi>(client);
            CommentsClient = RestService.For<IKaderCommentsApi>(client);
            CategoriesClient = RestService.For<IKaderCategoriesApi>(client);

            Console.WriteLine($"Logged in to userId '{tokenInfo.UserId}'");

            return tokenInfo;
        }

        public static HttpClient CreateClient()
        {
            var client = new HttpClient() {BaseAddress = new Uri(BaseUrl)};
            UsersClient = RestService.For<IKaderUsersApi>(client);
            
            return client;
        }

        public static async Task<User> GetRandomUserAsync(bool adminsOnly)
        {
            IEnumerable<User> users;

            if (adminsOnly)
            {
                users = await UsersClient.GetAdminsAsync();
            }
            else
            {
                users = await UsersClient.GetUsersAsync();
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

        public async Task<GroupView> GetRandomGroupAsync(string userId)
        {
            IEnumerable<GroupView> groups = await GroupsClient.GetGroupsAsync(userId);
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

        public async Task<PostView> GetRandomPostAsync()
        {
            IEnumerable<PostView> posts = await PostsClient.GetPostsAsync();
            List<PostView> postsList = posts.ToList();

            if (!postsList.Any())
            {
                throw new NullReferenceException("Posts is null");
            }

            var random = new Random();
            int randomValue = random.Next(postsList.Count);

            PostView postView = postsList[randomValue];
            Console.WriteLine($"Got post '{postView.Title}'");

            return postView;
        }

        public async Task<Category> GetRandomCategoryAsync()
        {
            IEnumerable<Category> categories = await CategoriesClient.GetCategories();
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

        public abstract Task SeedAsync();
    }
}
