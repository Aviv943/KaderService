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
        public static string BaseUrl;

        public static async Task<Seeds> CreateAsync(Type type, string baseUrl)
        {
            BaseUrl = baseUrl;
            return (Seeds)Activator.CreateInstance(type);
        }

        public static async Task<TokenInfo> LoginAsync()
        {
            var client = new HttpClient() { BaseAddress = new Uri(BaseUrl) };
            UsersClient = RestService.For<IKaderUsersApi>(client);
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

            return tokenInfo;
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
            var randomValue = random.Next(usersList.Count());

            return usersList[randomValue];
        }

        public async Task<GroupView> GetRandomGroupAsync()
        {
            IEnumerable<GroupView> groups = await GroupsClient.GetGroupsAsync();
            List<GroupView> groupsList = groups.ToList();

            if (!groupsList.Any())
            {
                throw new NullReferenceException("Groups is null");
            }

            var random = new Random();
            int randomValue = random.Next(groupsList.Count());

            return groupsList[randomValue];
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

            return postsList[randomValue];
        }

        public abstract Task SeedAsync();
    }
}
