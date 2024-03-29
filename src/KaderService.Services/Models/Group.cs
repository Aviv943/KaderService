﻿using System;
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
        public string GroupId { get; set; }

        //Type: ISR-Anubis-Club
        public string Name { get; set; }

        /// <summary>
        /// Sport/ School/ Cooking etc
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public string CategoryId { get; set; }

        public string Description { get; set; }
        
        public string Address { get; set; }

        public string Location { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        /// <summary>
        /// Private/ Public/ Invisible
        /// </summary>
        public GroupPrivacy GroupPrivacy { get; set; }
        
        public ICollection<User> Members { get; set; }

        public ICollection<User> Managers { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}