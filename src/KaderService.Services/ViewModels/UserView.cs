using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaderService.Services.ViewModels
{
    public class UserView
    {
        public string UserId { get; set; }
        
        public string UserName { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Rating { get; set; }

        public int NumberOfRating { get; set; }

        public string ImageUri { get; set; }
    }
}
