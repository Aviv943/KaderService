﻿using System.Collections.Generic;
using KaderService.Services.ViewModels;

namespace KaderService.Contracts.Responses
{
    public class GetPostsResponse
    {
        public List<PostView> PostViews { get; set; }
    }
}
