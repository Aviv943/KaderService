using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaderService.Services.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CommentId { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public User Creator { get; set; }

        [ForeignKey(nameof(PostId))]
        public string PostId { get; set; }
        
        public Post Post { get; set; }
    }
}