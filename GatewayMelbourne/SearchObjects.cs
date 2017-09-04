using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GatewayMelbourne
{ 

class SearchObjects

    {
        
        List<Location> searchResultList = new List<Location>();
       LocationObjects searchLocationObjs = new LocationObjects();
        List<Location> LocationList = new List<Location>();
        public void Search(string compareString)
        {
            
            LocationList = searchLocationObjs.getLocationList();
            foreach (Location searchItem in LocationList)
            {
                string normalisedLookupTable = Regex.Replace(searchItem.description, @"\s", "").ToLower()+ Regex.Replace(searchItem.description, @"\s", "");
                if (normalisedLookupTable.Contains(compareString) )
                {
                   searchResultList.Add(searchItem);
                  
                }
            }
            
        }
        
        public List<Location> getsearchResult()
        {
            return searchResultList;
        }
       
        }
    
}
