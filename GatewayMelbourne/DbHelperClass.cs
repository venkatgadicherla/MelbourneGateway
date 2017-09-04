using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace GatewayMelbourne
{
    class DbHelperClass
    {
        public async void CreateDbase()
        {

            try
            {
                App.path = Path.Combine(Windows.Storage.ApplicationData.
                Current.LocalFolder.Path, "GatewayMelbourne.sqlite");

                App.conn = new SQLite.Net.SQLiteConnection(new
                SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), App.path);
                //if (!CheckFileExists(App.path).Result)
                {
                    
                    App.conn.CreateTable<Location>();
                  
                    var msg = new MessageDialog("Database created Yahoo"+ CheckFileExists(App.path).Result);
                    await msg.ShowAsync();

                    //var locations = conn.Table<Locations>();

                    //    Locations Flinderstreet = new Locations { locationName = "FlindersStreetStation", category = "Landmarks", description = "Iconic Train station in the heart of Melbourne", favourite = "false", image = "" };
                    //    conn.Insert(Flinderstreet);

                    //// The following insert data in the database
                }
            }
            catch (Exception exp)
            {

            }

        }
        public void insertLocations()
        {
            Location GrtOcRd = new Location
            {
                locationName = "Great Ocean Rd",
                address= "Great Ocean Rd",
                category = "DayTours",

                smallimage = "DayTours//GreatOceanRd//Logo.png",
                description = "Whether you love a drive or serene nature or rugged sea sides Great Ocean Road  on the South West Coast of Victoria is the perfect destination.Only 3 hours drive from Melbourne is a place not to be missed if you are visiting Melbourne",
                keyWords = "Great Ocean Rd,West of Melbourne,12 Apostles,Beaches,drive,London bridge,Apollo bay,Light house,3 hours drive,Scenic beauty,DayTours",
                favourite = "False",
                page="itemGreatOceanDrive"
              
           };
            App.conn.Insert(GrtOcRd);
            Location MountBuller = new Location
            {
                locationName = "Mount Buller",
                address="Mount Buller",
                category = "DayTours",
                smallimage = "DayTours//MtBullerImages//MtBullerlogo.png",
                description = "  Want to be on top of the World, then head to Mount Buller,  the no one destination for a winter day out. Only 3 hours drive from Melbourne located in the Victorian Alps, this hill station boasts of breathtaking views and is one of the magnificent skiing resorts suiting any ability.",
                keyWords = "Snow,winter,skiing,Ice, Family, 3 hours,three hours,Victorian Alps,breathtaking views,ski resorts,East of Melbourne,Mount Buller,DayTours",
                favourite = "False",
                page="itemMountBuller"

            };
            App.conn.Insert(MountBuller);

            Location PhilipIsland = new Location
            {
                locationName = "Philip Island",
                address="Philip Island",
                category = "DayTours",
                smallimage = "DayTours//PhilipIslandImages//PengLogo.png",
                description = "  Phillip Island 140 Kms south of Melbourne is a popular tourist destination visited by around 3 million people annually.The Penguin Parade at Phillip Island Nature Park, in which little penguins come ashore in groups, attracts visitors from all over the world.Philip Island is also home to other attraction including the Whale watching,Seal Rock and Moto GP and would make a perfect summers day out. " ,
                keyWords = "Philip Island,Day Tour,South of Melbourne, Penguin Island, Whale watching,Dolphin watching,Seal rock,Penguin Parade,140 kms,100 kms,hundred kilometers,Beaches,Nature park,Motor gp, Motor Grand prix,",
                favourite = "False",
                page="ItemPhilipIsland"
            };
            App.conn.Insert(PhilipIsland);

            Location YarraValley = new Location
            {
                locationName = "Yarra Valley",
                address = "Yarra Valley",
                category = "DayTours",
                smallimage = "DayTours//YarraValley//Logo.png",
                description = "Just an hour from Melbourne, the Yarra Valley is a whole new experience  offering fine local food and wine, breathtaking scenery and indulgent adventures.The Yarra Valley is one of the world's premier wine growing regions with over 80 sensational cellar doors, superb restaurants, luxury accommodation, stunning scenery and  magnificient artwork.Places of interest include Tulip gardens,Puffing Billy,Mount Dandenong,Healsville Sanctuary. ",
                keyWords = "Yarra Valley,Day tours,East of Melbourne,One hour,1 hour,Tulip gardens,Puffing Billy,Mount Dandenong,Wineries,Wine tasting,Cheese,Healsville Sanctuary,Yarra river",
                favourite = "False",
                page="YarraValley"

            };
            App.conn.Insert(YarraValley);
            Location MelbourneZoo = new Location
            {
                locationName = "Melbourne Zoo",
                address="Melbourne zoo",
                category = "ParksAndGardens",
                smallimage = "ParksImages//MelbZoo//Logo.png",
                description = "Welcome to wilderness just 4 kilometers from Melbourne CBD. Melbourne zoo part of Zoos Victoria is one of the largest zoos in Australia.Melbourne zoo consists of animals from 350 diferrent species.The main attractions would be the Sumatran Tigers,Asian Elephants,African giraffes,Hippos,Rhinos and wide variety of animals ranging from local species to animals from allover the world.Melbourne Zoo is located amidst parks and picnic areas and would be an ideal family dayout.",
                keyWords = "Parks and gardens,Melbourne Zoo,zoo,Zoo in Melbourne,Zoo in Victoria",
                favourite = "False",
                page="itemMelbZoo"

            };
            App.conn.Insert(MelbourneZoo);
            Location WerribeeZoo = new Location
            {
                locationName = "Werribee Zoo",
                address="Weribee Zoo",
                category = "ParksAndGardens",
                smallimage = "ParksImages//WerribeeZoo//Logo.png",
                description = "Want to experience African Safari in Australia.Then head no where else but to Werribee.Werribee Zoo is a open range that gives you an oppurtuinty to experience the animals up close in the natural habitat like conditions.Weribee is only 30kms from Melbourne CBD and hosts a variety of events alround the year.Other attractions around the zoo include the Werribee Mansion Rose Gardens, Werribee river surroundings.",
                keyWords = "Werribee Zoo,Safari,Werribee Mansion,Werribee river,Rose gardens,Werribee,West of Melbourne,30 kms,Thirty kilometers,30 ks",
                favourite = "False",
                page="itemWerribeeZoo"

            };
            App.conn.Insert(WerribeeZoo);
            Location rbn = new Location
            {
                locationName = "Royal Botanical Gardens",
                address="Royal Botanical Gardens Melbourne",
                category = "ParksAndGardens",
                smallimage = "ParksImages//Rbn//Logo.png",
                description = "The Royal Botanical Garden of Melbourne  lovingly called as the 'Tan' by the locals is the best place find peace in the hustle bustle of the city.Located on the southern bank of the Yarra the Tan offers Tranquility,wilderness,serenity just few minutes from the city.The gardens consists of around 10,000 individual species of plants from around the world and also has herbarium.If you are a gardening enthusiasist this would be the place to be.Places to see include the Ornamental lake,Shrine of rememberance,Sydney Myer music bowl.",
                keyWords = "ParksAndGardens,Royal Botanical Garden,Tan,Botanical Garden,Ornamental lake,Shrine,Shrine of rememberance,Sydney Myer music bowl,Governor house,Herbarium,Yarra river",
                favourite = "False",
                page="itemBotanicalGarden"

            };
            App.conn.Insert(rbn);
            Location AquariumLoc = new Location
            {
                locationName = "Melbourne Aquarium",
                address= "Melbourne Aquarium",
                category = "ParksAndGardens",
                smallimage = "ParksImages//Aquarium//Logo.png",
                description = "Sea Life Aquarium on the banks of Yarra is one of the main attractions to visit in Melbourne.The aqurium is home to around ten thousand fish with many rare and amazing species of sea animals.Enter Sea Life Aquarium to get lost in to the world under water.",
                keyWords = "ParksAndGardens,Aqua park,Aquarium,Sea Life, Melbourne,Yarra Banks",
                favourite = "False",
                page = "Aquarium"

            };
            App.conn.Insert(AquariumLoc);
            Location Crown = new Location
            {
                locationName = "Crown Casino",
                address="Crown Casino Melbourne",
                category = "InTheCity",
                smallimage = "City//Crown//Logo.png",
                description = "Crown Entertainment complex is the place to be if you are looking for entertainment.Crown complex has the biggest Casino in the Southern Hemisphere and one of the biggest in the world.It has 4 luxurious hotels, Ballrooms, Theaters, Shopping, Gaming, Bowling, Restaurants.You name it, It has got it, including a waterfall.Located on the South Bank of Yarra Crown complex is a world in itself.",
                keyWords = "Crown Casino,South Bank,Vilage Cinemas,Games,Hotels,Restaurants,Shopping,BallRooms,City",
                favourite = "False",
                page = "itemCrown"

            };
            App.conn.Insert(Crown);

            Location FlindersStreet = new Location
            {
                locationName = "FlindersStreet",
                address="Flinders Street",
                category = "InTheCity",
                smallimage = "City//FlindersStreet//Logo.png",
                description =    "In the heart of the city lies the Flinders Street precinct. This would be the most happening place in Melbourne.The iconic Flinders Street Station,Federation Square,St Pauls Cathedral,the Princes Bridge and the Arts Precinct together form this precinct.Whether you are in for a better dining experience or to experience the antiquity of Flinders Street Station or experience arts at the Art Centre the Flinders Street and Arts centre precinct have got it for you. The Federation Sqaure and Arts centre can be termed as the culutral centre of  Melbourne and indeed Australia." ,
                keyWords = "Flinders Street,city,Federation Square, Fed Square,Arts Centre,Princes bridge,Flinders Street train Station,Railway Station,Melbourne Symphony Orchestra,MSO,Hammer Hall,St Pauls Cathedral,Yarra River,CBD",
                favourite = "False",
                page = "itemFlindersStreet"

            };
            App.conn.Insert(FlindersStreet);
            Location Market = new Location
            {
                locationName = "Queen Victoria Market",
                address="Queen Victoria Market",
                category = "InTheCity",
                smallimage = "City//Market//Logo.png",
                description = "Melbourne is a town of Markets.Melbourne has a number of markets in different parts of the city.Queen Victoria market is the most prominent of them.Established first in 1878 Queen Victoria Market has a rich and colourful history.Thousands of locals and tourists flock the market each week.Visit Queen Vic market popularly called by the locals for local and international street food, fresh organic produce and variety of souveniors.The Market is Open Five Days a Week Tuesday, and Thursday to Sunday.The Market is also open in the nights on Wednesdays.",
                keyWords = "Queen Victoria Market,Vic Market,Queen Vic Market,City",
                favourite = "False",
                page = "itemMarket"

            };
            App.conn.Insert(Market);
            Location StKilda = new Location
            {
                locationName = "StKilda",
                address="StKilda",
                category = "InTheCity",
                smallimage = "City//StKilda//Logo.png",
                description = "St Kilda is one of the inner suburbs of Melbourne prominent for its punk culture.St Kilda is home to many of Melbourne's famous visitor attractions including Luna Park, the Esplanade Hotel, Acland Street and Fitzroy Street. It is home to St Kilda Beach, Melbourne's most famous beach, several renowned theatres like Palais theatre,National theatre and the Astor theatre.St kilda also hosts several of Melbourne's big events and festivals. ",
                keyWords = "St Kilda Beach,Luna park,Fitzroy Street,Film Festival,Theatre,Acland Street,InTheCity,City",
                favourite = "False",
                page = "itemStkilda"

            };
            App.conn.Insert(StKilda);

            Location AusOpen = new Location
            {
                locationName = "Australian Open",
                address="Rod Laver Arena",
                category = "Events",
                smallimage = "Events//AusOpen//AusOpenImage.png",
                description = "When it comes to Australian Summer sports nothing gets bigger than Australain open Tennis.Staged over 2 weeks in late January Australian Open tennis is starting  event of international tennis season.Come to Melbourne to witness tennis history where more than half a million people visit the tennis courts every year.",

                keyWords = "Australain Open,Events,Aus Open,Tennis Summer,Sports,January Events,Jan Events,Rod Laver Arena,Margaret Court Arena",
                favourite = "False",
                page = "itemAusOpen"
            };
            App.conn.Insert(AusOpen);

            Location F1 = new Location
            {
                locationName = "Formula 1 Grand Prix",
                address="Lake side drive,Albert Park",
                category = "Events",
                smallimage = "Events//Formula1//Logo.png",
                description = "Come summer, Sports fans calendars are filled with events.F1 grandprix at Albert Park is the most important motor sport event happening in Melbourne.Australian Grand Prix is the first round of racing in the F1 season every year.This event takes place in March of every year over a period of 3 days though the buzz is in the wind one month prior to the event." ,

                keyWords = "Formula 1,GrandPrix,F1,Albert Park,Events,March,Motor Sport,Australian Grandprix",
                favourite = "False",
                page = "itemF1"
            };
            App.conn.Insert(F1);

            Location AFL = new Location
            {
                locationName = "AFL",
                address="Melbourne Cricket Ground",
                category = "Events",
                smallimage = "Events//AFL//Logo.png",
                description = "If you are in Melbourne,Victoria or anywhere in Australia you cannot ignore this. The Australian Rules Foot Ball league or AFL or Footy is at you all the time.AFL runs all winter and is in the news all year. Melbpourne Cricket Ground(MCG) is the Mecca strangely not cricket. Grand Finals in September is the pinacle of footy and every football fan and player want to be a part.The Grand finals is more or less like a festival which is celebrated over a week with a public holiday in Victoria before the game. Over 100,000 people flock to MCG on the Grand Final day with a parade of teams in the Streets of Melbourne one day prior and the winners parade after the game.",

                keyWords = "AFL,Events, Spring , September, Sport, Australian Rules Football,Football,Victoria,MCG,Melbourne Cricket ground,",
                favourite = "False",
                page = "itemAfl"
            };
            App.conn.Insert(AFL);
            Location MelbCup = new Location
            {
                locationName = "Melbourne Cup",
                address="Flemington Race Course",
                category = "Events",
                smallimage = "Events//MelbourneCup//Logo.png",
                description = "The Melbourne Cup is Australia's most prestigious annual Thoroughbred horse race, 'The Race that stops the nation' as it is dubbed.This is the pinnacle event of the Spring Racing Carnival in Australia.People and racing enthusiasists from all over the globe come to watch this race.Melbourne Cup takes place on the first Tuesday of November and not be missed if you are in Melbourne.",
                keyWords = "Melbourne Cup,The Race that stops a nation,Horse Race,Spring Carnival,Throughbred racing",
                favourite = "False",
                page = "itemMelbCup"
            };
            App.conn.Insert(MelbCup);

            //Location catIntheCity = new Location
            //{
            //    locationName = "InTheCity",
                
            //    smallimage = "City//FlindersStreet//Logo.png",
            //    description = "",
            //    keyWords = "FlindersStreet,Crown,StKilda",
            //    favourite = "False",
            //    page = "InTheCity"
            //};
            //App.conn.Insert(catIntheCity);
            //Location catIntheCity = new Location
            //{
            //    locationName = "InTheCity",

            //    smallimage = "City//FlindersStreet//Logo.png",
            //    description = "",
            //    keyWords = "FlindersStreet,Crown,StKilda",
            //    favourite = "False",
            //    page = "InTheCity"
            //};
            //App.conn.Insert(catIntheCity);
        }

        public void retrieveData()
        {
           var list = App.conn.Table<Location>();
        }
        public void deleteData()
        {
            try
            {
                App.conn.DropTable<Location>();
                App.conn.CreateTable<Location>();
            }
            catch(Exception exp)
            {

            }
            
            
        }
        public List<Location> retrieveLocation(int id)
        {
            var l1 =  App.conn.Query<Location>("Select * from Location where id=" + id);
            return l1;
            
            
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
