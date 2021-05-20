using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;

namespace KaderService.Services.ViewModels
{
    public class PostView
    {
        public string PostId { get; set; }

        public PostType Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Location { get; set; }

        public DateTime Created { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public int CommentsCount { get; set; }

        public ICollection<CommentView> Comments { get; set; }

        public ICollection<string> ImagesUri { get; set; }

        public UserView Creator { get; set; }
        public Category Category { get; set; }
    }
}
