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
                await LoginAsync();
                await GroupsClient.CreateGroupAsync(group);
            }
        }

        public virtual List<T> GetData<T>()
        {
            var groups = new List<Group>
            {
                new()
                {
                    Name = "Area 51",
                    Category = "Sport",
                    Description = "Area 51 Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "בית הערבה 8, ראשון לציון"
                },
                new()
                {
                    Name = "Jokes",
                    Category = "Sport",
                    Description = "Jokes Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "הרצל 33, תל אביב"
                },
                new()
                {
                    Name = "Students In Colman",
                    Category = "Sport",
                    Description = "Students In Colman Group Description",
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Address = "אושה 5, ראשון לציון"
                },
                new()
                {
                    Name = "Cars Pishpeshok",
                    Category = "Sport",
                    Description = "Cars Pishpeshok Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "החלמונית 22, ראשון לציון"
                },
                new()
                {
                    Name = "Junior Developers Petah-Tikva",
                    Category = "Sport",
                    Description = "Junior Developers Petah-Tikva Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "שדה נחום 11, ראשון לציון"
                },
                new()
                {
                    Name = "Senior Developers Holon",
                    Category = "Sport",
                    Description = "Senior Developers Holon Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "חניתה 1, ראשון לציון"
                },
                new()
                {
                    Name = "Budapest for travelers",
                    Category = "Sport",
                    Description = "Budapest for travelers Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "ירמיהו 48, תל אביב"
                },
                new()
                {
                    Name = "Prague for travelers",
                    Category = "Sport",
                    Description = "Prague for travelers Group Description",
                    GroupPrivacy = GroupPrivacy.Invisible,
                    Address = "העצמאות 12, אשדוד"
                },
                new()
                {
                    Name = "Cheap stuff",
                    Category = "Sport",
                    Description = "Cheap stuff Group Description",
                    GroupPrivacy = GroupPrivacy.Private,
                    Address = "הרצל 1, תל אביב"
                },
                new()
                {
                    Name = "Bitcoin mining",
                    Category = "Crypto Coin",
                    Description = "Bitcoin mining Group Description",
                    GroupPrivacy = GroupPrivacy.Public,
                    Address = "שונית 2, רמלה"
                }
            };

            return (List<T>) Convert.ChangeType(groups, typeof(List<Group>));
        }
    }
}