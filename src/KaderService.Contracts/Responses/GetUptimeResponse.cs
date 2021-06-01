using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaderService.Contracts.Responses
{
    public class GetUptimeResponse
    {
        public string Formatted { get; set; }

        public string TotalSeconds { get; set; }

        public string TotalMinutes { get; set; }

        public string TotalHours { get; set; }
        
        public string TotalDays { get; set; }
    }
}