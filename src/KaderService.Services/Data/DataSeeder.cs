using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                    await UploadPostImageAsync(requestedPost, postId, user);

                    foreach (Comment requestedComment in requestedPost.Comments)
                    {
                        user = await _dataCreator.LoginRandomUserAsync();
                        await _dataCreator.GroupsService.AddUserRoleToGroupMemberAsync(groupId, user, "Member");
                        await _dataCreator.CommentsService.CreateCommentAsync(requestedComment, user, postId);
                    }
                }
            }
        }

        private async Task UploadPostImageAsync(Post requestedPost, string postId, User user)
        {
            try
            {
                string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = Path.Combine(executableLocation, "Data\\images", requestedPost.ImagesUri.First());
                await using Stream ms = new FileStream(filePath, FileMode.Open);

                var file = new FormFile(ms, 0, ms.Length, null!, "Image.jpg")
                {
                    Headers = new HeaderDictionary()
                };

                await _dataCreator.PostsService.CreatePostImageAsync(postId, user, file);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not upload image to user {user.Id}, ex: '{e.Message}'");
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
                                new() { Content = "Call me I can Help you! 056-43324561" }

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
                                new() { Content = "Can I come to take it now???" }

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
                                new() { Content = "I'm calling you right now" }
                            }
                        }
                    }
                },
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
                                new() { Content = "Call me I can Help you! 056-43324561" }

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
                                new() { Content = "Can I come to take it now???" }

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
                                new() { Content = "I'm calling you right now" }
                            }
                        }
                    }
                },
                #endregion

                #region Books
                new Group
                {
                    Name = "Harry Potter Reading Club",
                    Description = "Reading Harry Potter's books together",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Books"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "handing over the first book in the series",
                            Description = "Harry Potter and the Philosopher's Stone - I've read it twice",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "HarryP.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would be happy to read it!" },
                                new() { Content = "Wow I've looked for this book for months!" },
                                new() { Content = "Can you provide your phone number?" },
                                new() { Content = "Is it the origonal edition?" },
                                new() { Content = "call me I have to read it!!! 056-2342132?" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for the book Harry Potter and the Half-Blood Prince",
                            Description = "I've not read it yet and I'm looking for this book for a long time!",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Half_Blood.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have this book, not bad...  call me 069-32119434" },
                                new() { Content = "I think you can get in a library in the north of the city" },
                                new() { Content = "In the book shop of Salim for a good price!" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Reading event",
                            Description = "I'm planning a reading event at the college library, anyone who wants to come can call me! we will read Harry Potter's books call me : 075-12318765",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Harrybooks.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Cool I will come" },
                                new() { Content = "Can I come with my girlfriend? She does not go to college" },
                                new() { Content = "You plan to read all of them?" }
                            }
                        }
                    }
                },
                new Group
                {
                    Name = "Paulo Coelho books",
                    Description = "This group its for Paulo Coelho books lovers!!!",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Sport"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "handing over The Alchemist (1988)",
                            Description = "The Alchemist is a psychological novel, or so it would like you to think. call me is you want it 050-325650989",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Alchemist.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have heard about the book a lot, I would love to have it!" },
                                new() { Content = "can I have them? my number 056-6530454" },
                                new() { Content = "Can you provide your phone number?" },
                                new() { Content = "I've called you please get back to me when you can!" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for the book 'The Devil and Miss Prym'",
                            Description = "I have heard good reviews about him and I am interested to read it",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Miss.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I do not think the book is interesting. You should not waste time on it" },
                                new() { Content = "I have the book. I live down the street and welcome to come."},
                                new() { Content = "You can come - 060-43409859" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Selling a book of Paulo Coelho for 10$",
                            Description = "By the River Piedra I Sat Down and Wept (1994)",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "River.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I want it please" },
                                new() { Content = "I'm willing to pay 20$" },
                                new() { Content = "Please provide tour number!" }

                            }
                        }
                    }
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
                                new() { Content = "Is the TRX is new? can I come and see it?" }
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
                                new() { Content = "You can come and take mine:)" }
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
                                new() { Content = "Thank you! I will come with my friend!" }
                            }
                        }
                    }
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
                                new() { Content = "I've called you please get back to me when you can!" }
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
                                new() { Content = "You can come run with me 045-22316513" }
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
                                new() { Content = "What is the price for a month?" }
                            }
                        }
                    }
                },
                #endregion

                #region Cars
                new Group
                {
                    Name = "Pishpeshuk Car",
                    Description = "Private, commercial, trucks, jeeps, motorcycles, scooters, motorcycles, ATVs, collectibles and car accessories",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Cars"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for someone with a battery.",
                            Description = "Super Urgent",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "carbattery.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Hello, I'm another half hour in your area if that's relevant" },
                                new() { Content = "Call me, I'm on the way, 055-3143948" }

                            }
                        },
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Hending over a New car mirrors",
                            Description = "New car mirrors for free",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "carmirrors.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would love to have them please" },
                                new() { Content = "Is it possible to arrive in the next hour?" },
                                new() { Content = "Can I come now?" }

                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "New steering wheel covers for a seat cover",
                            Description = "I have a new steering wheel cover that I do not need, I would be happy to replace in exchange for a seat cover",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "carsteeringwheelcovers.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Call me and we'll talk about it - 0543948573" },
                                new() { Content = "still relevant?" },
                                new() { Content = "Coming to Ashdod?" }
                            }
                        }
                    }
                },
                new Group
                {
                    Name = "Fixing your car for good price!",
                    Description = "Do not spend a lot of money on car repairs! Look for professionals here",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Cars"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for someone who can fix engine",
                            Description = "There are strange noises when I start the car - I have Chevrolet",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "chevrolet.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I also had the same problem Garry fix it for me for a good price! his number is 050-6788900" },
                                new() { Content = "Hi, call me I can help you 052-2123323" },
                                new() { Content = "Come to my garage I'll check it for free!" }

                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Free Car wipers in my garage",
                            Description = "Free Car wipers in my garage - just come and take its good for all cars",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Car_wipers.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "greate I will come " },
                                new() { Content = "Can I take 2? for me and my wife? :)" }

                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "car wash for 5$",
                            Description = "All money will be donated for children shelter ",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "wash.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "You are a good person!!" },
                                new() { Content = "I will donate! " },
                                new() { Content = "A lovely initiative" },
                                new() { Content = "I'm willing to volunteer to wash with you" }
                            }
                        }
                    }
                },
                new Group
                {
                    Name = "Luxury cars lovers",
                    Description = "If you like luxury cars this is the group for you",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Cars"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "A test drive in a Mercedes",
                            Description = "I'm planning to buy a Mercedes. I am interested in a test drive to test the driving experience in this vehicle. Can anyone give me a test drive?",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Mercedes.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Yes I have the c-class 2019 you can take a ride with me 054-3260120" },
                                new() { Content = "I now that in the 'Cars Test' car lot you can pay for test drive!" },
                                new() { Content = "Can you provide your phone number?" },
                                new() { Content = "I've called you please get back to me when you can!" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for the book 'The Devil and Miss Prym'",
                            Description = "I have heard good reviews about him and I am interested to read it",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "Miss.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I do not think the book is interesting. You should not waste time on it" },
                                new() { Content = "I have the book. I live down the street and welcome to come."},
                                new() { Content = "You can come - 060-43409859" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Selling Audi Q3 Sportback for 30000$",
                            Description = "Year - 2019, new wheels, 23000 km, ready for trade",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "audi.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would like to ask you some questions, can you provid your number? or call me 065-2132443" },
                                new() { Content = "Please provide your number!" }

                            }
                        }
                    }
                },
                #endregion

                #region Cooking
                new Group
                {
                    Name = "Moms Cooking Togather!",
                    Description = "Cooking group for moms you can ask sell hand over all about coocking!!",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Cooking"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking pancake machine",
                            Description = "Need it for my for my daughter's birthday on sunday",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "pancack.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have two of them you can come and borrow for sunday" },
                                new() { Content = "Hi, call me I can help you 052-2176323" },
                                new() { Content = "Hi dear I have this machine, I dont know if it still works but you can take it." }

                            }
                        },
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Handing over gluten free flour",
                            Description = "I have 3k of gluten free flour you can come - my number 056-3250984",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "glutenFree.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "Greate I will come for it. My son is allergic to gluten and it is really great" },
                                new() { Content = "Is it Kosher? I am the wife of the rabbi :)" }

                            }
                        },
                        new()
                        {
                            Type = PostType.Offer,
                            Title = "Selling my Ninja machine for 100$",
                            Description = "New machine got it for my birthday and I dont need it",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "ninja.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "This is very expensive!! I better buy in store" },
                                new() { Content = "I can pay 70$, is it good for you ?" },
                                new() { Content = "I would love to buy it from you my number is 056-21655323" },
                                new() { Content = "I'm willing to pay it! call me!!" }
                            }
                        }
                    }
                },
                new Group
                {
                    Name = "Cooking lovers",
                    Description = "If you like to coock and bake we would like to join to our group",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Cooking"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for a baking course",
                            Description = "I'm a beginner baker and I really like baking. I would love to find a course that will expand my knowledge.",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "bakeCourse.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I attend Mary's course on Thursdays. Really good you should join." },
                                new() { Content = "You can join to my course every monday in Yerushalmi St.!" }
                            }
                        },
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking to buy a baking oven",
                            Description = "I offer 50$ need an oven, not have to be new.",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "oven.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I'm selling my oven for 45$, call me 076-737099" },
                                new() { Content = "In the store that is located down the street you can buy for 20$."},
                                new() { Content = "I have one for free I dont need it, It work good!" }
                            }
                        }
                    }
                },
                #endregion

                #region Food
                new Group
                {
                    Name = "Hamburger Lovers",
                    Description = "WE are hamburger lovers",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Food"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for a meat grinder",
                            Description = "I need grinder for the hamburger meat",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "hamburger.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have two of them you can come and take one!!" },
                                new() { Content = "HI!! come and take mine." }
                            }
                        }
                    }
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
                                new() { Content = "Is the TRX is new? can I come and see it?" }
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
                                new() { Content = "You can come and take mine:)" }
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
                                new() { Content = "Thank you! I will come with my friend!" }
                            }
                        }
                    }
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
                                new() { Content = "I've called you please get back to me when you can!" }
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
                                new() { Content = "You can come run with me 045-22316513" }
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
                                new() { Content = "What is the price for a month?" }
                            }
                        }
                    }
                },
                new Group
                {
                    Name = "Water sports surfing Community",
                    Description = "The Israeli Surfing Community - Surfers SAP | Windsurfing surfing",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון",
                    Category = await _dataCreator.CategoriesService.GetCategoryAsync("Sport"),
                    Posts = new List<Post>
                    {
                        new()
                        {
                            Type = PostType.Request,
                            Title = "Looking for a surfboard",
                            Description = "Anyone have a surfboard for tomorrow",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "surfboard.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I have a surfboard available for tomorrow if you want, talk to me" },
                                new() { Content = "Tomorrow I am surfing at Keshot Beach in Ashdod, you are welcome to come." }
                            }
                        },
                        new()
                        {
                            Type = PostType.Handover,
                            Title = "Handing over a surfboard wax",
                            Description = "surfboard wax for free",
                            Address = "בית הערבה 6, ראשון לציון",
                            ImagesUri = new List<string> { "surfboardwax.jpg" },
                            Comments = new List<Comment>
                            {
                                new() { Content = "I would love to have it please" },
                                new() { Content = "Do you have any more left?" },
                                new() { Content = "When can I come and pick up?" }
                            }
                        }
                    }
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
                                new() { Content = "Please attach some photos of your watermelons" }
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
                                new() { Content = "If you want some green onion and tomato, contact me" }
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
                                new() { Content = "Is white color good?" }
                            }
                        }
                    }
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
                                new() { Content = "Move out from Tel Aviv! we don't need people like you!" }
                            }
                        }
                    }
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
                                new() { Content = "Welcome to come and collect, Mazal Tov!" }
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
                                new() { Content = "Wish i could send my father, he is so old in his mind..." }
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
                                new() { Content = "I'm willing to pay for you for a professional help" }
                            }
                        }
                    }
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
                                new() { Content = "Did you manage?" }
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
                                new() { Content = "I don't have enough money to buy one, with i could afford a bicycle these corna virus days" }
                            }
                        }
                    }
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
                                new() { Content = "Can you provide the SKU of the item? I would like to get some extra information about this one" }
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
                                new() { Content = "I would like to get 2 of them please, I don't need the whole set" }
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
                                new() { Content = "You are welcome to come and get it, but i need it for the next 2 weeks" }
                            }
                        }
                    }
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
                                new() { Content = "Please attach some extra photos" }
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
                                new() { Content = "Can you save it for me for tomorrow?" }
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
                        }
                    }
                }
                #endregion       
            };
        }
    }
}