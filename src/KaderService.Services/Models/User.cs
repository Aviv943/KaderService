using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace KaderService.Services.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            MemberInGroups = new List<Group>();
            Posts = new List<Post>();
            Comments = new List<Comment>();
            ManagerInGroups = new List<Group>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public double Rating { get; set; }

        public int NumberOfRatings { get; set; }

        public DateTime Created { get; set; }

        public string ImageUri { get; set; }

        public ICollection<Post> Posts { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public ICollection<Group> MemberInGroups { get; set; }

        public ICollection<Group> ManagerInGroups { get; set; }
    }
}