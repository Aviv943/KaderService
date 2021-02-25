using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KaderService.Services.Models
{
    public class User
    {
        //[Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public ICollection<Group> Groups { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ImageUrl { get; set; }
    }
}
