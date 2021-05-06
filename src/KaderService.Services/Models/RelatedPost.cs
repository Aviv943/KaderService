using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaderService.Services.Models
{
    public class RelatedPost
    {
        public RelatedPost(string userId, string postId)
        {
            UserId = userId;
            PostId = postId;
        }

        [Required]
        [DataType(DataType.Custom)]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Custom)]
        public string PostId { get; set; }
    }
}
