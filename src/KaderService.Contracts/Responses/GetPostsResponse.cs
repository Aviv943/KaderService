using System.Collections.Generic;
using KaderService.Services.Models;

namespace KaderService.Contracts.Responses
{
    public class GetPostsResponse
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
