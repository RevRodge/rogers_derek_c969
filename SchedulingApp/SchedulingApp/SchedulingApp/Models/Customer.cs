using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    // Customer and related info.
    // Address details are pulled in via joins to the address, city, and country tables.
    public class Customer
    {
        // pk from customer table
        public int CustomerId { get; set; }
        
        // display string
        public string CustomerName { get; set; } = string.Empty;

        // fk back to address table
        public int AddressId { get; set; }

        //True if customer "active" in DB
        public bool Active { get; set; }


        // Address fields (from joined SELECT)
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;

        // fk to the city table (needed for inserts/ updates)
        public int CityId { get; set; }

        // display strings
        public string CityName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

    }
    
}
