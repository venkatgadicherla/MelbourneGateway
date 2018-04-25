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
        //Class Location is the key class in the app. It embodies all the attributes of the locations in the app and used to make the database using Sql lite
        //It is also used for the search algorithm and to store favourite status of the location.
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
