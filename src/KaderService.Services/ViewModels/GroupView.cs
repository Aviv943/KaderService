﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Constants;

namespace KaderService.Services.ViewModels
{
    public class GroupView
    {
        public string GroupId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string MainLocation { get; set; }

        public DateTime Created { get; set; }

        public GroupPrivacy GroupPrivacy { get; set; }

        public int MembersCount { get; set; }

        public int ManagersCount { get; set; }

        public int PostsCount { get; set; }
    }
}