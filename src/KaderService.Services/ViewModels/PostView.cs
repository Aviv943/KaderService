using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;

namespace KaderService.Services.ViewModels
{
    public class PostView
    {
        public string PostId { get; set; }

        public PostType Type { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime Created { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public int CommentsCount { get; set; }

        public ICollection<string> ImagesUri { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Rating { get; set; }

        public int NumberOfRating { get; set; }
    }
}
