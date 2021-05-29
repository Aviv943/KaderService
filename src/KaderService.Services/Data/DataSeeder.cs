using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using Microsoft.AspNetCore.Http;

namespace KaderService.Services.Data
{
    public class DataSeeder
    {
        private static DataCreator _dataCreator;

        public DataSeeder(DataCreator dataCreator)
        {
            _dataCreator = dataCreator;
        }

        public async Task CreateData()
        {
            List<Group> requestedGroups = await GetNewData();

            foreach (Group requestedGroup in requestedGroups)
            {
                var group = new Group
                {
                    Name = requestedGroup.Name,
                    Description = requestedGroup.Description,
                    GroupPrivacy = requestedGroup.GroupPrivacy,
                    Address = requestedGroup.Address,
                    CategoryId = requestedGroup.Category.Id
                };

                User user = await _dataCreator.LoginRandomUserAsync();
                string groupId = await _dataCreator.GroupsService.CreateGroupAsync(group, user);

                foreach (Post requestedPost in requestedGroup.Posts)
                {
                    var post = new Post
                    {
                        Type = requestedPost.Type,
                        Title = requestedPost.Title,
                        Description = requestedPost.Description,
                        Address = requestedPost.Address
                    };

                    user = await _dataCreator.LoginRandomUserAsync();
                    await _dataCreator.GroupsService.AddUserRoleToGroupMemberAsync(groupId, user, "Member");
                    string postId = await _dataCreator.PostsService.CreatePostAsync(post, user, groupId);

                    try
                    {
                        string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string filePath = Path.Combine(executableLocation, "Data\\images", requestedPost.ImagesUri.First());
                        await using Stream ms = new FileStream(filePath, FileMode.Open);

                        var file = new FormFile(ms, 0, ms.Length, null!, "Image.jpg")
                        {
                            Headers = new HeaderDictionary(),
                        };

                        await _dataCreator.PostsService.CreatePostImageAsync(postId, user, file);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Could not upload image to user {user.Id}, ex: '{e.Message}'");
                    }

                    foreach (Comment requestedComment in requestedPost.Comments)
                    {
                        user = await _dataCreator.LoginRandomUserAsync();
                        await _dataCreator.GroupsService.AddUserRoleToGroupMemberAsync(groupId, user, "Member");
                        await _dataCreator.CommentsService.CreateCommentAsync(requestedComment, user, postId);
                    }
                }
            }
        }

        private async Task<List<Group>> GetNewData()
        {
            return new()
            {
                #region Beauty
                new Group
                {
                    Name = "Pretty woman and man!",
                    Description = "Nails Polish, Haircuts and more - Tel Aviv",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Beauty"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for someone who can get my nails done today!!",
                            Description = "Super Urgent",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "nails.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Hi, I have an opening in seven PM you want to come?" },
                                new() { Content = "Ask Sisi she can help you with this!!!" },
                                new() { Content = "Call me I can Help you! 056-43324561" },

                            }
                        },
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Hending over a pedicure machine",
                            Description = "pedicure machine - like a new!!!!",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "pedicure.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Wow!!! I want it please my number is 054-250230" },
                                new() { Content = "Can I come to take it now???" },

                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "New haircut for free!!!!!!",
                            Description = "I'm new hair dresser offering this as part of my learning as part my intership. My number is 050-31223983",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "haircut.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Wow I will definitely come!!" },
                                new() { Content = "Does it appeal to both sexes" },
                                new() { Content = "OMG!!! An amazing initiative" },
                                new() { Content = "I'm calling you right now" },
                            }
                        },
                    },
                },
                #endregion

                #region Sport
                new Group
                {
                    Name = "Getting back in shape",
                    Description = "Getting back in shape after COVID19 period",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Sport"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "handing over sports equipment",
                            Description = "I hand over TRX, skipping rope, weight 2kg each.",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "weight.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would be happy to get it,can you call me? 076-345565" },
                                new() { Content = "Wow Looks great, can I have them?" },
                                new() { Content = "Can you provide your phone number? I would to get more photos from you!" },
                                new() { Content = "Is it possible to take a single item?" },
                                new() { Content = "Is the TRX is new? can I come and see it?" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for resistance bands loop set",
                            Description = "I need the set for my for cross fit lesson tomorrow - just for a few hours",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "bands.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have one you can take it today, I live down the street" },
                                new() { Content = "I think you can get one in the store that located near" },
                                new() { Content = "You can come and take mine:)" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Pilates lesson",
                            Description = "I offer you a pilates lesson for free! this sunday! call me 054-3325645",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "pilates.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Wow this is awsome I will definitely come!!" },
                                new() { Content = "That's really nice of you" },
                                new() { Content = "Thank you! I will come with my friend!" },
                            }
                        },
                    },
                },
                new Group
                {
                    Name = "Running together",
                    Description = "Getting back in shape after COVID19 period",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Sport"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Someone needs running shoes",
                            Description = "I'm handing over - if you want just come to get it!! size 43",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "shoes.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would be happy to get them! please provide your number!" },
                                new() { Content = "can I have them? my number 056-6530454" },
                                new() { Content = "Can you provide your phone number?" },
                                new() { Content = "I've called you please get back to me when you can!" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for someone to run with",
                            Description = "I'm from north Tel Aviv, looking someone to run on mornings",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "run.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Wow I'm some glad you've posted this! I can run with you!" },
                                new() { Content = "You have the 'Bees' running group the run each morning near the Yarkon!" },
                                new() { Content = "You can come run with me 045-22316513" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "TRX Group",
                            Description = "New TRX Group in mondays! firs lesson for free!!",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "TRX.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I will join!!" },
                                new() { Content = "Its only on mondays?? " },
                                new() { Content = "What is the price for two subscribers?" },
                                new() { Content = "What is the price for a month?" },
                            }
                        },
                    },
                },
                #endregion

                #region Garden
                new Group
                {
                    Name = "Flowers In Gush Dan",
                    Description = "A group for flowers lovers in Tel Aviv and Gush Dan",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Garden"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Watermelon seeds",
                            Description = "I have a lot of watermelon seeds from the season, feel free to come pick some",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "watermelon.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Watermelon? Holy Crap! On my way" },
                                new() { Content = "Is it possible to pick up some for next month?" },
                                new() { Content = "Please attach some photos of your watermelons" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Looking for offers for plants I can grow this season",
                            Description = "I am a beginner grower, I have 5 hours of sun in the garden every day, I would love suggestions for plants that I can grow",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "seeds.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "If you want some green onion and tomato, contact me" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for garden planters",
                            Description = "For a study project in botany",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "planters.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have some I can contribute" },
                                new() { Content = "What size do you need? Maybe I have some that will fit" },
                                new() { Content = "I have some used ones, help you?" },
                                new() { Content = "Is white color good?" },
                            }
                        }
                    },
                },
                new Group
                {
                    Name = "Gardening in Tel Aviv",
                    Description = "Any help you need for your garden in Tel Aviv",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Garden"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Need seeds",
                            Description = "Just moved to tel aviv and I'm building my new garden",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "seeds.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Welcome to Tel Aviv, you can arrive and take from my garden" },
                                new() { Content = "I have some tomato seeds, is it help?" },
                                new() { Content = "Move out from Tel Aviv! we don't need people like you!" },
                            }
                        }
                    },
                },
                #endregion

                #region Home
                new Group
                {
                    Name = "Kiryat Ganim neighborhood in Rishon Lezion",
                    Description = "A group for mutual help to the community in the Kiryat Ganim neighborhood in Rishon Lezion",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Home"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Prayer equipment",
                            Description = "Looking for prayer equipment for my son who is going up to Torah next week",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Torah.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Welcome to come and collect, Mazal Tov!" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Offers help for the elderly in the neighborhood center",
                            Description = "I am a beginner grower, I have 5 hours of sun in the garden every day, I would love suggestions for plants that I can grow",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "elderly.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Well Done!" },
                                new() { Content = "Very Nice, I'll tell me grandmother" },
                                new() { Content = "What ages?" },
                                new() { Content = "My grandfather will arrive next week" },
                                new() { Content = "Wish i could send my father, he is so old in his mind..." },
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Asks for help hanging a TV in the room",
                            Description = "I am an older person and I do not have the ability to depend on myself, I would love your help",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "tv.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "No worries man, I'll arrive tomorrow morning, be safe!" },
                                new() { Content = "I can arrive as well" },
                                new() { Content = "I'm willing to pay for you for a professional help" },
                            }
                        }
                    },
                },
                new Group
                {
                    Name = "Negba 24 building in Rishon Lezion",
                    Description = "This group for the building at 24 Negba Street in Rishon Lezion",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "נגבה 24, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Home"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Need onions",
                            Description = "My parents are coming to visit in two hours and I am missing the shade and the supermarket is already closed",
                            Address = "נגבה 24, ראשון לציון",
                            ImagesUri = new List<string> { "onion.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Welcome to come pick me up, I'm from apartment 13" },
                                new() { Content = "Also invited to me, 7th floor apartment 28" },
                                new() { Content = "Did you manage?" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Offers bicycle repair services",
                            Description = "I have a bicycle repair business and I offer my free help for building members",
                            Address = "נגבה 24, ראשון לציון",
                            ImagesUri = new List<string> { "bicycle.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Well Done!" },
                                new() { Content = "I don't have enough money to buy one, with i could afford a bicycle these corna virus days" },
                            }
                        },
                    },
                },
                #endregion

                #region Tools
                new Group
                {
                    Name = "Business equipment in Rishon Lezion",
                    Description = "Area 51 Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Tools"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Drill for handover",
                            Description = "I hand over my drill that I have had in the last two years, whoever needs it is welcome to contact and I will be happy to bring it to him.",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "drill.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would be happy to get it" },
                                new() { Content = "Is it new?" },
                                new() { Content = "Can you provide the SKU of the item? I would like to get some extra information about this one" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "New set of screwdrivers for handover",
                            Description = "A set of new screwdrivers for hand over, were used for a total of two weeks, removed due to non-conformity",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "screwdrivers.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "What are the sizes of the screwdrivers?" },
                                new() { Content = "Are these screwdrivers match for home using?" },
                                new() { Content = "I would like to get 2 of them please, I don't need the whole set" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for a ladder",
                            Description = "Looking for a ladder for early next week. Willing to pay if needed",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "ladder.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have one for sale if you need. I've never used it before, it's brand new." },
                                new() { Content = "You are welcome to come and get it, but i need it for the next 2 weeks" },
                            }
                        },
                    },
                },
                new Group
                {
                    Name = "Bostch tools ONLY",
                    Description = "This group is for Bostch tools only",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Tools"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Old saw for handover",
                            Description = "I have an old chainsaw that I no longer need, anyone who wants to take is welcome",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "oldsaw.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "What is the SKU or Model please?" },
                                new() { Content = "How old is it?" },
                                new() { Content = "Please attach some extra photos" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Handover my bostch sanding",
                            Description = "Bosch Extreme model, old but still working",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "bostchsanding.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Can you save it for me for tomorrow?" },
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for a ladder",
                            Description = "Looking for a any ladder for simple use",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "bostchladder.jpg" },
                            Comments = new List<Comment>()
                        },
                    },
                }
                #endregion       
            };
        }
    }
}