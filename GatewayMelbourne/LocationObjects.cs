using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayMelbourne
{
    class LocationObjects
    {
        public List<Location> LocationList=new List<Location>();
       public
            LocationObjects()
        {
            var localLocationList = App.conn.Table<Location>();
            foreach(Location loc in localLocationList)
            {
                LocationList.Add(loc);
            }
            
        }
        public List<Location> getLocationList()
        {
            return LocationList;
        }
    }
}
