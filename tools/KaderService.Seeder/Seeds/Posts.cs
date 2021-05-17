using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;

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
                GroupView group = await GetRandomGroupAsync();
                
                try
                {
                    await PostsClient.CreatePostAsync(post, group.GroupId);
                    Console.WriteLine($"Post created '{post.Title}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Post could not be created, ex: '{e.Message}'");
                }
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
                    Address = "נירים 3, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Address = "הרצל 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Food",
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Address = "דיזינגוף 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Address = "אלי כהן 1, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Address = "הרצל 14, בת ים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Address = "דיזינגוף 5, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Food",
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Address = "בית הערבה 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Food",
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Address = "אושה 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Food",
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Address = "הרצל 14, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Address = "שונית 6, רמלה",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Address = "הרצל 14, ירושלים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Address = "אלי כהן 14, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Address = "הרצל 2, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Address = "שדה נחום 2, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Address = "העצמאות 2, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Address = "הרצל 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Harp",
                    Description = "Looking for Harp Please",
                    Address = "הרצל 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Piano",
                    Description = "Looking for Piano Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Cello",
                    Description = "Looking for Cello Please",
                    Address = "דיזינגוף 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Bass drum",
                    Description = "Looking for Bass drum Please",
                    Address = "נגבה 20, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Bass Guitar",
                    Description = "Looking for Bass Guitar Please",
                    Address = "נגבה 18, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Guitar",
                    Description = "Looking for Guitar Please",
                    Address = "נגבה 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Music",
                    Title = "Harmonica",
                    Description = "Looking for Harmonica Please",
                    Address = "שדה ורבורג 16, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Bell",
                    Description = "Looking for Bell Please",
                    Address = "עמיר 7, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Music",
                    Title = "Tuba",
                    Description = "Looking for Tuba Please",
                    Address = "עמיר 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Music",
                    Title = "Microphone",
                    Description = "Looking for Microphone Please",
                    Address = "העצמאות 22, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Mouse",
                    Description = "Looking for Mouse Please",
                    Address = "הרצל 9, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "HeadPhones",
                    Description = "Looking for HeadPhones Please",
                    Address = "הרצל 8, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Category = "Technology",
                    Title = "USB Stick",
                    Description = "Looking for USB Stick Please",
                    Address = "שדה נחום 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Router",
                    Description = "Looking for Router Please",
                    Address = "כפר חרוב 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Category = "Technology",
                    Title = "Laptop",
                    Description = "Looking for Laptop Please",
                    Address = "כפר חרוב 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Category = "Technology",
                    Title = "Web cam",
                    Description = "Looking for Web cam Please",
                    Address = "הרצל 4, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
            };

            return (List<T>)Convert.ChangeType(posts, typeof(List<Post>));
        }
    }
}