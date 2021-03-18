using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaderService.Services.ViewModels
{
    public class CommentView
    {
        public string CommentId { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Rating { get; set; }

        public int NumberOfRating { get; set; }

        public string PostId { get; set; }

    }
}
