using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Services.Models
{
    public class User :IdentityUser
    {
        public User()
        {
            Groups = new List<Group>();
            Posts=new List<Post>();
            Comments=new List<Comment>();
        }

        //[Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ImageUri { get; set; }
    }
}
