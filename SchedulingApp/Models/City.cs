using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    public class City
    {
        // pk from city table
        public int CityId { get; set; }
        // display string
        public string Name { get; set; } = string.Empty;
        //fk back to country table
        public int CountryId { get; set; }
    }
}
