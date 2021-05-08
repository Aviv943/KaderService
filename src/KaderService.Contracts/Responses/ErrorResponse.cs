using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaderService.Contracts.Responses
{
    public class ErrorResponse
    {
        public string InternalCode { get; set; }

        public string Message { get; set; }
    }
}
