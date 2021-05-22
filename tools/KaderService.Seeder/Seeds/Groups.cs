using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;

namespace KaderService.Seeder.Seeds
{
    public class Groups : Seeds
    {
        public override async Task SeedAsync()
        {
            List<Group> groups = GetData<Group>();

            foreach (Group group in groups)
            {
                await LoginAsync();
                Category category = await GetRandomCategoryAsync();
                group.CategoryId = category.Id;

                try
                {
                    await GroupsClient.CreateGroupAsync(group);
                    Console.WriteLine($"Group created '{group.Name}'");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Group could not be created, ex: '{e.Message}'");
                }
            }
        }

        public virtual List<T> GetData<T>()
        {
            var groups = new List<Group>
            {
                new()
                {
                    Name = "Area 51",
                    Description = "Area 51 Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון"
                },
                new()
                {
                    Name = "Jokes",
                    Description = "Jokes Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "הרצל 33, תל אביב"
                },
                new()
                {
                    Name = "Students In Colman",
                    Description = "Students In Colman Group Description",
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Address = "אושה 5, ראשון לציון"
                },
                new()
                {
                    Name = "Cars Pishpeshok",
                    Description = "Cars Pishpeshok Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "החלמונית 22, ראשון לציון"
                },
                new()
                {
                    Name = "Junior Developers Petah-Tikva",
                    Description = "Junior Developers Petah-Tikva Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "שדה נחום 11, ראשון לציון"
                },
                new()
                {
                    Name = "Senior Developers Holon",
                    Description = "Senior Developers Holon Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "חניתה 1, ראשון לציון"
                },
                new()
                {
                    Name = "Budapest for travelers",
                    Description = "Budapest for travelers Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "ירמיהו 48, תל אביב"
                },
                new()
                {
                    Name = "Prague for travelers",
                    Description = "Prague for travelers Group Description",
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Address = "העצמאות 12, אשדוד"
                },
                new()
                {
                    Name = "Cheap stuff",
                    Description = "Cheap stuff Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "הרצל 1, תל אביב"
                },
                new()
                {
                    Name = "Bitcoin mining",
                    Description = "Bitcoin mining Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "שונית 2, רמלה"
                },
                new()
                {
                    Name = "Street Group",
                    Description = "Build your own fake Facebook Status",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון"
                },
                new()
                {
                    Name = "Bodybuilders",
                    Description = "The bodybuilders",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "הרצל 33, תל אביב"
                },
                new()
                {
                    Name = "Inverters",
                    Description = "For ppl who like to invest",
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Address = "אושה 5, ראשון לציון"
                },
                new()
                {
                    Name = "Coffe liker",
                    Description = "Do you like coffe? join us!",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "החלמונית 22, ראשון לציון"
                },
                new()
                {
                    Name = "FSD rules",
                    Description = "Are you FSD Dev?",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "שדה נחום 11, ראשון לציון"
                },
                new()
                {
                    Name = "Israel vs Gaza",
                    Description = "A WAR",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "חניתה 1, ראשון לציון"
                },
                new()
                {
                    Name = "Credit cards",
                    Description = "Credit cards club",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "ירמיהו 48, תל אביב"
                },
                new()
                {
                    Name = "Travelers",
                    Description = "Travelers Group",
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Address = "העצמאות 12, אשדוד"
                },
                new()
                {
                    Name = "Tech club",
                    Description = "For ppl who like tech",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "הרצל 1, תל אביב"
                },
                new()
                {
                    Name = "Apple fanboys",
                    Description = "Mac, iPhone and more",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "שונית 2, רמלה"
                }
            };

            return (List<T>) Convert.ChangeType(groups, typeof(List<Group>));
        }
    }
}