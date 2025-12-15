using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    public class Address
    {
        // pk from address table
        public int AddressId { get; set; }
        
        // todo: first line mandatory, second address line optional
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        
        // fk back to city table
        public int CityId { get; set; }
        
        // display strings
        public string PostalCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
