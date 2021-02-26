using System;

namespace KaderService.Services.Models
{
    public class Comment
    {
        //[Key]
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}