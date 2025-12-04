using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    namespace SchedulingApp.Models
    {
        public class Customer
        {
            public int CustomerId { get; set; }
            public string CustomerName { get; set; } = string.Empty;
            public int AddressId { get; set; }
            public bool Active { get; set; }

            //TODO Add detailed address get/sets
        }
    }
}
