using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingApp.Models
{
    public class Country
    {
        // PK from Country table
        public int CountryId { get; set; }
        //Country display name
        public string Name { get; set; } = string.Empty;
    }
}
