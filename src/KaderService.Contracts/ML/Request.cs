using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaderService.Contracts.ML
{
    public class Request
    {
        public List<ItemsCustomers> ItemCustomersList { get; set; }

        public List<int> AllItemsIds { get; set; }

        public string CustomerNumber { get; set; }
    }
}
