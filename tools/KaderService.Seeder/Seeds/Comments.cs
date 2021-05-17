using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;

namespace KaderService.Seeder.Seeds
{
    public class Comments : Seeds
    {
        public override async Task SeedAsync()
        {
            List<Comment> comments = GetData<Comment>();

            foreach (Comment comment in comments)
            {
                await LoginAsync();
                PostView post = await GetRandomPostAsync();
                
                try
                {
                    await CommentsClient.CreateCommentAsync(comment, post.PostId);
                    Console.WriteLine($"Comment created '{comment.Content}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Comment could not be created, ex: '{e.Message}'");
                }

            }
        }

        public virtual List<T> GetData<T>()
        {
            var comments = new List<Comment>
            {
                new()
                {
                    Content = "Fancy a rice cake?",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Oh, yeah. Cheers, pal.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Hey. Here’s your pen back.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Ta, mate.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Wow! You fixed my bike! Thanks a bunch!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "You’ll go get me a coffee? Thanks a million! Really. I just don’t have the time!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "A £75 fine for a bill that’s one day late? Great. Thanks a million.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Oh, and Laurie? Really, thanks so much for covering my shift at work",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Hey, Freya! Thank you so much for Alex’s birthday present. I’m sure he’ll love it!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Hey! Excuse me. You dropped your phone!, Oh! Thanks a lot!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "You rule!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Flowers? For me? How thoughtful!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Don’t mention it.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Not at all!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "It’s nothing.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "That’s all right.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "It’s my pleasure.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Thanks so much for the positive feedback.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Hey this is really a great lesson for me thanks a lot.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Thanks so much for your huge efforts it helped me a lot",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Thanks for the positive feedback. It’s great to hear that learners like you are benefiting from it.",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "thanks!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "No. Thank YOU!",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Thanks so much",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "Thank you. This is very, very useful. ",
                    Created = DateTime.Now
                },
                new()
                {
                    Content = "No worries!",
                    Created = DateTime.Now
                }
            };

            return (List<T>)Convert.ChangeType(comments, typeof(List<Comment>));
        }
    }
}