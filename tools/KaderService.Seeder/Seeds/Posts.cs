using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;

namespace KaderService.Seeder.Seeds
{
    public class Posts : Seeds
    {
        public override async Task SeedAsync()
        {
            List<Post> posts = GetData<Post>();

            foreach (Post post in posts)
            {
                await LoginAsync();
                Group group = await GetRandomGroupAsync();
                await PostsClient.CreatePostAsync(post, group.GroupId);
            }
        }

        public virtual List<T> GetData<T>()
        {
            var posts = new List<Post> {
                new()
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Technology",
                    Description = "Looking for Technology Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Food",
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Food",
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Location = "Ramat-Gan",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Harp",
                    Description = "Looking for Harp Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Piano",
                    Description = "Looking for Piano Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Cello",
                    Description = "Looking for Cello Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Bass drum",
                    Description = "Looking for Bass drum Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Bass Guitar",
                    Description = "Looking for Bass Guitar Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Guitar",
                    Description = "Looking for Guitar Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Harmonica",
                    Description = "Looking for Harmonica Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Bell",
                    Description = "Looking for Bell Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Tuba",
                    Description = "Looking for Tuba Please",
                    Location = "Lod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Microphone",
                    Description = "Looking for Microphone Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Mouse",
                    Description = "Looking for Mouse Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "HeadPhones",
                    Description = "Looking for HeadPhones Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "USB Stick",
                    Description = "Looking for USB Stick Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Router",
                    Description = "Looking for Router Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Laptop",
                    Description = "Looking for Laptop Please",
                    Location = "Tel-Aviv",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Web cam",
                    Description = "Looking for Web cam Please",
                    Location = "Ashdod",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
            };

            return (List<T>)Convert.ChangeType(posts, typeof(List<Post>));
        }
    }
}