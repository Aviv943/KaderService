﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;

namespace KaderService.Seeder.Seeds
{
    public class Comments : Seeds
    {
        public override async Task SeedAsync()
        {
            List<Comment> comments = GetData<Comment>();

            for (var i = 0; i < 40; i++)
            {
                TokenInfo tokenInfo = await LoginAsync();

                foreach (Comment comment in comments)
                {
                    try
                    {
                        PostView post = await GetRandomPostAsync();

                        if (post.Creator.UserId == tokenInfo.UserId)
                        {
                            continue;
                        }

                        await CommentsClient.CreateCommentAsync(comment, post.PostId);
                        Console.WriteLine($"Comment created '{comment.Content}'");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Comment could not be created, ex: '{e.Message}'");
                    }
                }
            }
        }

        public virtual List<T> GetData<T>()
        {
            var comments = new List<Comment>
            {
                new() { Content = "Fancy a rice cake?" },
                new() { Content = "Oh, yeah. Cheers, pal." },
                new() { Content = "Hey. Here’s your pen back." },
                new() { Content = "Ta, mate." },
                new() { Content = "Wow! You fixed my bike! Thanks a bunch!" },
                new() { Content = "You’ll go get me a coffee? Thanks a million! Really. I just don’t have the time!" },
                new() { Content = "A £75 fine for a bill that’s one day late? Great. Thanks a million." },
                new() { Content = "Oh, and Laurie? Really, thanks so much for covering my shift at work" },
                new() { Content = "Hey, Freya! Thank you so much for Alex’s birthday present. I’m sure he’ll love it!" },
                new() { Content = "Hey! Excuse me. You dropped your phone!, Oh! Thanks a lot!" },
                new() { Content = "You rule!" },
                new() { Content = "Flowers? For me? How thoughtful!" },
                new() { Content = "Don’t mention it." },
                new() { Content = "Not at all!" },
                new() { Content = "It’s nothing." },
                new() { Content = "That’s all right." },
                new() { Content = "It’s my pleasure." },
                new() { Content = "Thanks so much for the positive feedback." },
                new() { Content = "Hey this is really a great lesson for me thanks a lot." },
                new() { Content = "Thanks so much for your huge efforts it helped me a lot" },
                new() { Content = "Thanks for the positive feedback. It’s great to hear that learners like you are benefiting from it." },
                new() { Content = "thanks!" },
                new() { Content = "No. Thank YOU!" },
                new() { Content = "Thanks so much" },
                new() { Content = "Thank you. This is very, very useful. " },
                new() { Content = "No worries!" },
           };

            return (List<T>)Convert.ChangeType(comments, typeof(List<Comment>));
        }
    }
}