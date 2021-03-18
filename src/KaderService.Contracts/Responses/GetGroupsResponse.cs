using System.Collections.Generic;
using KaderService.Services.ViewModels;

namespace KaderService.Contracts.Responses
{
    public class GetGroupsResponse
    {
        public List<GroupView> GroupView { get; set; }
    }
}
