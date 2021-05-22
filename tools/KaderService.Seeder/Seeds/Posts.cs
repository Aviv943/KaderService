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
                TokenInfo token = await LoginAsync();
                GroupView group = await GetRandomGroupAsync(token.UserId);

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
                    Title = "Technology",
                    Description = "Looking for Technology Please",
                    Address = "נירים 3, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Address = "הרצל 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Address = "דיזינגוף 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Address = "אלי כהן 1, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Address = "הרצל 14, בת ים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Address = "דיזינגוף 5, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Address = "בית הערבה 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Address = "אושה 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Address = "הרצל 14, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Address = "שונית 6, רמלה",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Address = "הרצל 14, ירושלים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Address = "אלי כהן 14, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Address = "הרצל 2, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Address = "שדה נחום 2, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Address = "העצמאות 2, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Address = "הרצל 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Harp",
                    Description = "Looking for Harp Please",
                    Address = "הרצל 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Piano",
                    Description = "Looking for Piano Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cello",
                    Description = "Looking for Cello Please",
                    Address = "דיזינגוף 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Bass drum",
                    Description = "Looking for Bass drum Please",
                    Address = "נגבה 20, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bass Guitar",
                    Description = "Looking for Bass Guitar Please",
                    Address = "נגבה 18, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Guitar",
                    Description = "Looking for Guitar Please",
                    Address = "נגבה 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Harmonica",
                    Description = "Looking for Harmonica Please",
                    Address = "שדה ורבורג 16, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bell",
                    Description = "Looking for Bell Please",
                    Address = "עמיר 7, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Tuba",
                    Description = "Looking for Tuba Please",
                    Address = "עמיר 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Microphone",
                    Description = "Looking for Microphone Please",
                    Address = "העצמאות 22, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Mouse",
                    Description = "Looking for Mouse Please",
                    Address = "הרצל 9, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "HeadPhones",
                    Description = "Looking for HeadPhones Please",
                    Address = "הרצל 8, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "USB Stick",
                    Description = "Looking for USB Stick Please",
                    Address = "שדה נחום 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Router",
                    Description = "Looking for Router Please",
                    Address = "כפר חרוב 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Laptop",
                    Description = "Looking for Laptop Please",
                    Address = "כפר חרוב 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Web cam",
                    Description = "Looking for Web cam Please",
                    Address = "הרצל 4, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Technology",
                    Description = "Looking for Technology Please",
                    Address = "נירים 3, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Address = "הרצל 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Address = "דיזינגוף 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Address = "אלי כהן 1, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Address = "הרצל 14, בת ים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Address = "דיזינגוף 5, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Address = "בית הערבה 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Address = "אושה 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Address = "הרצל 14, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Address = "שונית 6, רמלה",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Address = "הרצל 14, ירושלים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Address = "אלי כהן 14, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Address = "הרצל 2, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Address = "שדה נחום 2, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Address = "העצמאות 2, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Address = "הרצל 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Harp",
                    Description = "Looking for Harp Please",
                    Address = "הרצל 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Piano",
                    Description = "Looking for Piano Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cello",
                    Description = "Looking for Cello Please",
                    Address = "דיזינגוף 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Bass drum",
                    Description = "Looking for Bass drum Please",
                    Address = "נגבה 20, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bass Guitar",
                    Description = "Looking for Bass Guitar Please",
                    Address = "נגבה 18, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Guitar",
                    Description = "Looking for Guitar Please",
                    Address = "נגבה 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Harmonica",
                    Description = "Looking for Harmonica Please",
                    Address = "שדה ורבורג 16, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bell",
                    Description = "Looking for Bell Please",
                    Address = "עמיר 7, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Tuba",
                    Description = "Looking for Tuba Please",
                    Address = "עמיר 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Microphone",
                    Description = "Looking for Microphone Please",
                    Address = "העצמאות 22, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Technology",
                    Description = "Looking for Technology Please",
                    Address = "נירים 3, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Address = "הרצל 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Address = "דיזינגוף 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Address = "אלי כהן 1, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Address = "הרצל 14, בת ים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Address = "דיזינגוף 5, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Address = "בית הערבה 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Address = "אושה 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Address = "הרצל 14, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Address = "שונית 6, רמלה",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Address = "הרצל 14, ירושלים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Address = "אלי כהן 14, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Address = "הרצל 2, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Address = "שדה נחום 2, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Address = "העצמאות 2, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Address = "הרצל 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Harp",
                    Description = "Looking for Harp Please",
                    Address = "הרצל 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Piano",
                    Description = "Looking for Piano Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cello",
                    Description = "Looking for Cello Please",
                    Address = "דיזינגוף 8, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Bass drum",
                    Description = "Looking for Bass drum Please",
                    Address = "נגבה 20, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bass Guitar",
                    Description = "Looking for Bass Guitar Please",
                    Address = "נגבה 18, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Guitar",
                    Description = "Looking for Guitar Please",
                    Address = "נגבה 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Harmonica",
                    Description = "Looking for Harmonica Please",
                    Address = "שדה ורבורג 16, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bell",
                    Description = "Looking for Bell Please",
                    Address = "עמיר 7, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Tuba",
                    Description = "Looking for Tuba Please",
                    Address = "עמיר 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Microphone",
                    Description = "Looking for Microphone Please",
                    Address = "העצמאות 22, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Mouse",
                    Description = "Looking for Mouse Please",
                    Address = "הרצל 9, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "HeadPhones",
                    Description = "Looking for HeadPhones Please",
                    Address = "הרצל 8, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "USB Stick",
                    Description = "Looking for USB Stick Please",
                    Address = "שדה נחום 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Router",
                    Description = "Looking for Router Please",
                    Address = "כפר חרוב 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Laptop",
                    Description = "Looking for Laptop Please",
                    Address = "כפר חרוב 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Web cam",
                    Description = "Looking for Web cam Please",
                    Address = "הרצל 4, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Technology",
                    Description = "Looking for Technology Please",
                    Address = "נירים 3, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Apples",
                    Description = "Looking for apples Please",
                    Address = "הרצל 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Melons",
                    Description = "Looking for melons Please",
                    Address = "דיזינגוף 14, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Garlic",
                    Description = "Looking for garlic Please",
                    Address = "אלי כהן 1, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Ketchup",
                    Description = "Looking for ketchup Please",
                    Address = "הרצל 14, בת ים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Fish",
                    Description = "Looking for Fish Please",
                    Address = "דיזינגוף 5, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Milk",
                    Description = "Looking for Milk Please",
                    Address = "בית הערבה 6, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Cheese",
                    Description = "Looking for Cheese Please",
                    Address = "אושה 5, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Eggs",
                    Description = "Looking for Eggs Please",
                    Address = "הרצל 14, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "BasketBall",
                    Description = "Looking for BasketBall Please",
                    Address = "שונית 6, רמלה",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "Jump Ropes",
                    Description = "Looking for Jump Ropes Please",
                    Address = "הרצל 14, ירושלים",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Offer,
                    Title = "SoftBalls",
                    Description = "Looking for SoftBalls Please",
                    Address = "אלי כהן 14, חולון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Bats",
                    Description = "Looking for Bats Please",
                    Address = "הרצל 2, תל אביב",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "FootBalls",
                    Description = "Looking for FootBalls Please",
                    Address = "שדה נחום 2, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Handover,
                    Title = "Hockey Sticks",
                    Description = "Looking for Hockey Sticks Please",
                    Address = "העצמאות 2, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Hula Hoops",
                    Description = "Looking for Hula Hoops Please",
                    Address = "העצמאות 4, אשדוד",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                },
                new()
                {
                    Type = PostType.Request,
                    Title = "Gloves",
                    Description = "Looking for Gloves Please",
                    Address = "הרצל 10, ראשון לציון",
                    Created = DateTime.Now,
                    ImagesUri = new List<string>()
                }
            };

            return (List<T>)Convert.ChangeType(posts, typeof(List<Post>));
        }
    }
}