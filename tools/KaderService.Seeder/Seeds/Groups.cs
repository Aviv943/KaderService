using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                await GroupsClient.CreateGroupAsync(group);
            }
        }

        public virtual List<T> GetData<T>()
        {
            var groups =  new List<Group>
            {
                new()
                {
                    Name = "Area 51",
                    Category = "Sport",
                    Description = "Area 51 Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public,
                },
                new()
                {
                    Name = "Jokes",
                    Category = "Sport",
                    Description = "Jokes Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private
                },
                new()
                {
                    Name = "Students In Colman",
                    Category = "Sport",
                    Description = "Students In Colman Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Invisible
                },
                new()
                {
                    Name = "Cars Pishpeshok",
                    Category = "Sport",
                    Description = "Cars Pishpeshok Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private
                },
                new()
                {
                    Name = "Junior Developers Petah-Tikva",
                    Category = "Sport",
                    Description = "Junior Developers Petah-Tikva Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public
                },
                new()
                {
                    Name = "Senior Developers Holon",
                    Category = "Sport",
                    Description = "Senior Developers Holon Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private
                },
                new()
                {
                    Name = "Budapest for travelers",
                    Category = "Sport",
                    Description = "Budapest for travelers Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public
                },
                new()
                {
                    Name = "Prague for travelers",
                    Category = "Sport",
                    Description = "Prague for travelers Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Invisible
                },
                new()
                {
                    Name = "Cheap stuff",
                    Category = "Sport",
                    Description = "Cheap stuff Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Private
                },
                new()
                {
                    Name = "Bitcoin mining",
                    Category = "Crypto Coin",
                    Description = "Bitcoin mining Group Description",
                    Searchable = true,
                    GroupPrivacy = GroupPrivacy.Public
                },
            };

            return (List<T>) Convert.ChangeType(groups, typeof(List<Group>));
        }
    }
}
