using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KaderService.Services.Constants;
using Microsoft.VisualBasic;

namespace KaderService.Services.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        //Type: Request help/ Offer help/ Handover an item
        public PostType Type { get; set; }

        //Category: Sport/ School/ Cooking
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string Location { get; set; }
        public string[] Images { get; set; }
        public User Creator { get; set; }
        public string GroupId { get; set; }
        public DateTime Created { get; set; }
    }
}
