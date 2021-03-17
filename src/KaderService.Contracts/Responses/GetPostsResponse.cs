using System;
using System.Collections.Generic;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.ViewModels;

namespace KaderService.Contracts.Responses
{
    public class GetPostsResponse
    {
        public List<GroupView> GroupView { get; set; }
    }
}
