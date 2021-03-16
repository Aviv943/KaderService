using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Models;

namespace KaderService.Seeder.Seeds
{
    public class Comments : Seeds
    {
        public override async Task SeedAsync()
        {
            List<Comment> comments = GetData<Comment>();
            User user = await GetRandomUserAsync();

            foreach (Comment comment in comments)
            {
                Post post = await GetRandomPostAsync();
                await CommentsClient.CreateCommentAsync(comment, user, post.PostId);
            }
        }

        public virtual List<T> GetData<T>()
        {
            var comments = new[]
            {
                new Comment
                {
                    Content = "Fancy a rice cake?",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Oh, yeah. Cheers, pal.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Hey. Here’s your pen back.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Ta, mate.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Wow! You fixed my bike! Thanks a bunch!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "You’ll go get me a coffee? Thanks a million! Really. I just don’t have the time!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "A £75 fine for a bill that’s one day late? Great. Thanks a million.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Oh, and Laurie? Really, thanks so much for covering my shift at work",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Hey, Freya! Thank you so much for Alex’s birthday present. I’m sure he’ll love it!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Hey! Excuse me. You dropped your phone!, Oh! Thanks a lot!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "You rule!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Flowers? For me? How thoughtful!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Don’t mention it.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Not at all!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "It’s nothing.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "That’s all right.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "It’s my pleasure.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Thanks so much for the positive feedback.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Hey this is really a great lesson for me thanks a lot.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Thanks so much for your huge efforts it helped me a lot",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Thanks for the positive feedback. It’s great to hear that learners like you are benefiting from it.",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "thanks!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "No. Thank YOU!",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Thanks so much",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "Thank you. This is very, very useful. ",
                    Created = DateTime.Now
                },
                new Comment
                {
                    Content = "No worries!",
                    Created = DateTime.Now
                }
            };

            return (List<T>)Convert.ChangeType(comments, typeof(List<Comment>));
        }
    }
}