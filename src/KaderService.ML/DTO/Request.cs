using System.Collections.Generic;

namespace KaderService.ML.DTO
{
    public class Request
    {
        public List<ItemsCustomers> RelatedPostsList { get; set; }

        public List<string> PostsIds { get; set; }

        public string UserId { get; set; }
    }
}
