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

        public UserView UserView { get; set; }

        public string PostId { get; set; }

    }
}
