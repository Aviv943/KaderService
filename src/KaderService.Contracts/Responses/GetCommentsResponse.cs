using System.Collections.Generic;
using KaderService.Services.ViewModels;

namespace KaderService.Contracts.Responses
{
    public class GetCommentsResponse
    {
        public List<CommentView> CommentViews { get; set; }
    }
}
