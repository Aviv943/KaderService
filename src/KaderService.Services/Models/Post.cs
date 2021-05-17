using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KaderService.Services.Constants;

namespace KaderService.Services.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
            ImagesUri = new List<string>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PostId { get; set; }

        //Type: Request help/ Offer help/ Handover an item
        public PostType Type { get; set; }

        /// <summary>
        /// Sport/ School/ Cooking etc
        /// </summary>
        public string Category { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Address { get; set; }

        public string Location { get; set; }

        public User Creator { get; set; }
        
        [ForeignKey(nameof(GroupId))]
        public string GroupId { get; set; }

        public Group Group { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now;
        
        public ICollection<Comment> Comments { get; set; }
        
        public ICollection<string> ImagesUri { get; set; }
    }
}