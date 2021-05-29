using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaderService.Services.Models;

namespace KaderService.Services.Data
{
    public class SeederInfo
    {
        public Category Category { get; set; }

        public List<Group> Groups { get; set; }
    }
}