using System.Collections.Generic;

namespace KaderService.ML.DTO
{
    public class Request
    {
        public List<ItemsCustomers> RelatedPostsList { get; set; }

        public List<int> PostsNumbers { get; set; }

        public int UserNumbers { get; set; }
    }
}
