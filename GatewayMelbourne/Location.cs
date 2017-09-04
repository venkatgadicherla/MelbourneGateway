using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayMelbourne
{
    public class Location
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int locationId { get; set; }
        [NotNull]
        public string locationName { get; set; }
        [NotNull]
        public string address { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string keyWords { get; set; }
        
        public string smallimage { get; set; }

        public string page { get; set; }

        public string favourite { get; set; }
       


    }
}
