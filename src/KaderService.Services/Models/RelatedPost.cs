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
        public RelatedPost(int userNumber, int postNumber)
        {
            UserNumber = userNumber;
            PostNumber = postNumber;
        }

        [Required]
        [DataType(DataType.Custom)]
        public int UserNumber { get; set; }

        [Required]
        [DataType(DataType.Custom)]
        public int PostNumber { get; set; }
    }
}
