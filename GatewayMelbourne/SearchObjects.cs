using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GatewayMelbourne
{

    class SearchObjects

    {
        // This class forms the basis to compare the search string with data in database
        // This class has 2 main and critical methods.
        // Method 1 is the Search method
        // Method 2 is the getSearchResult which returns the result of the Search operation
        List<Location> searchResultList = new List<Location>();
        LocationObjects searchLocationObjs = new LocationObjects();
        List<Location> LocationList = new List<Location>();
        public  void Search(string compareString)
        {
            //This method would take the string and would comapre with the description of the locations and puts the results  in the searchResultList List
            try
            {
                LocationList = searchLocationObjs.getLocationList();
                foreach (Location searchItem in LocationList)
                {
                    string normalisedLookupTable = Regex.Replace(searchItem.description, @"\s", "").ToLower() + Regex.Replace(searchItem.keyWords, @"\s", "").ToLower()+Regex.Replace(searchItem.category, @"\s", "").ToLower();
                    if (normalisedLookupTable.Contains(compareString))
                    {
                        searchResultList.Add(searchItem);

                    }
                }
            }
            catch (System.Exception exp)
            {
               
            }
        }

        public List<Location> getsearchResult()
        {
            return searchResultList;
        }
       
        }
    
}
