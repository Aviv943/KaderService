using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KaderService.Services.Constants;

namespace KaderService.Services.Models
{
    public class Group
    {
        public Group()
        {
            GroupPrivacy = GroupPrivacy.Public;
            Members = new List<User>();
            Managers = new List<User>();
            Posts = new List<Post>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        //Type: ISR-Anubis-Club
        public string Name { get; set; }

        //Category: Sport/ School/ Cooking
        public string Category { get; set; }
        public string Description { get; set; }
        public string MainLocation { get; set; }

        public bool Searchable { get; set; }

        //Privacy: Private/ Public/ Invisible
        public GroupPrivacy GroupPrivacy { get; set; }
        public ICollection<User> Members { get; set; }

        //todo ask yoni what the fuck does it want from my relationships god dam fucking shit motherfucker
        public ICollection<User> Managers { get; set; }

        public ICollection<Post> Posts { get; set; }

        //  public DateTime GroupCreated { get; set; }
    }
}