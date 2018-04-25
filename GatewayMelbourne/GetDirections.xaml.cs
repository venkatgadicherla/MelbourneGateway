using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GatewayMelbourne
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GetDirections : Page
    {

        public string address = "Melbourne";// This Variable is used used for basic address to launch the pin
        char[] pinButtonletter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();//This variable is used to assign letters to the pins added to the map
        public List<PinDropped> currentPins = new List<PinDropped>();// whenever a pin added to the map the pin object is  added to the current pins list.
        public List<MapRouteView> routesList = new List<MapRouteView>();//All the route legs are added to this list and displayed
        public List<MapIcon> iconList = new List<MapIcon>();//This is the list of all the map icons added to the mapp
        public List<StackPanel> draggedPanelsList = new List<StackPanel>();//This is the list of location panels that is added to the map
        
        public class PinDropped  // This class is used contain the location co-ordinates and name which would be address or location name
        {

            public double latitude { get; set; }
            public double longitude { get; set; }

            public string pinTitle { get; set; }

        }
      

      
        public bool isInternetConnected()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null &&
            connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
       
        }

        public GetDirections()
        {
            this.InitializeComponent();
        }

        // The following event is triggered on navigation to the GetDirection page from the location pages
        // On navigation to this page from a location page the users location is obtained where available and distance to that location 
        //and directions are calculated.
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            
            try
            {
                clearMap();
                base.OnNavigatedTo(e);
               
                  if (e.Parameter != null)
                    {
                        if (isInternetConnected())
                           {
                        PinDropped userLocationPin = await getUserLocation();
                        PinDropped destinationPin = await getDestinationPin(e.Parameter.ToString());
                        if ((userLocationPin!=null) &&  destinationPin != null)
                        {
                            AddPin(userLocationPin);// A pin is added to the map with users location. If no user location available Melbourne CBD is taken as the starting point
                            AddPin(destinationPin);//a destination pin which is the location pages from which the user has navigated from is also added here
                            await getDirectionsonMapandPage();// Directions are calculated and added to the map and the page

                        }

                    }
                        else
                        {

                        var msg = new MessageDialog("Directions could not be acheived as Internet services not available at the moment");
                        await msg.ShowAsync();
                        }

                    }
               
            }
            catch (Exception exp)
            {
                var msg = new MessageDialog(exp.ToString());
                await msg.ShowAsync();
            }



        }
        public void clearMap()
        {
            // This method used to clear the map of all the icons and clear all directions in the page

            int routeCounter = 0;
            while (routeCounter < routesList.Count)// This loop clears the route legs from the map
            {
                MelbMap.Routes.Remove(routesList[routeCounter]);
                routeCounter++;
                currentPins.Clear();
            }
            int iconCounter = 0;
            while (iconCounter < iconList.Count)// This loop clears the map icons from the map i.e, pins
            {
                MelbMap.MapElements.Remove(iconList[iconCounter]);


                iconCounter++;
            }
            currentPins.Clear();
            foreach (StackPanel pnl in draggedPanelsList)// this loop sets up the locations to be draggable again once the map has been cleared
            {
                pnl.CanDrag = true;

                ToolTipService.SetToolTip(pnl, "Drag on to the map");

            }
            draggedPanelsList.Clear();
            panelLowerDirections.Children.Clear();




        }


        // The following method would generate the pin for the destination when location address or name is given
        private async Task<PinDropped> getDestinationPin(string locationName)
        {
            //This method is used to generate a pin when a address is supplied.
            PinDropped destinationPin = new PinDropped();
            BasicGeoposition melbourne = new BasicGeoposition();
            try
            {
               
                melbourne.Latitude = -37.8136;
                melbourne.Longitude = 144.9631;
                Geopoint hintpoint = new Geopoint(melbourne);
                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(locationName, hintpoint);
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    try
                    {
                        if (result.Locations.Count > 0)
                        {
                            destinationPin.latitude = result.Locations[0].Point.Position.Latitude;
                            destinationPin.longitude = result.Locations[0].Point.Position.Longitude;
                            destinationPin.pinTitle = locationName;
                        }
                        else
                        {
                            destinationPin = null;
                            //var msg1 = new MessageDialog(result.Status.ToString());
                            //await msg1.ShowAsync();
                        }
                    }
                    catch (Exception exp)
                    {
                        var expMsg = new MessageDialog(" inner " + result.Status.ToString()); //exp.ToString());
                        await expMsg.ShowAsync();
                    }

                }
                else
                {
                    destinationPin = null;
                    var msg1 = new MessageDialog(result.Status.ToString());
                    await msg1.ShowAsync();
                }
            
            }
            catch (Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }
            return destinationPin;
        }

       

       

        //The following event when the map is loaded. In this method the City of Melbourne is made the centre of the map.
        public async void getCentre(double latitude, double longitude, int zoom)
        {
            try
            {

                var center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = latitude,

                    Longitude = longitude

                });
                await MelbMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, zoom));
            }
            catch(Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }
        }

        private async void MelbMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                getCentre(-37.818514, 144.967855, 25000);
            }
            catch (Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }
        }

        // The following method is used to get the location of the user based on their location. 
        public async Task<PinDropped> getUserLocation()
        {
            PinDropped pin = new PinDropped();
            if (isInternetConnected())
            {

               
                var accessStatus = await Geolocator.RequestAccessAsync();
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        var geoLocator = new Geolocator();
                        geoLocator.DesiredAccuracy = PositionAccuracy.High;
                        Geoposition pos = await geoLocator.GetGeopositionAsync();
                        Double latitude = Convert.ToDouble(pos.Coordinate.Point.Position.Latitude);
                        Double longitude = Convert.ToDouble(pos.Coordinate.Point.Position.Longitude); ;
                        pin.latitude = latitude;
                        pin.longitude = longitude;
                        pin.pinTitle = await getAddressFromLocation(latitude, longitude);

                        break;

                    case GeolocationAccessStatus.Denied:
                        var msg2 = new MessageDialog("Access to the user location is " + accessStatus.ToString() + ". So Melbourne City is taken as the starting location");

                        await msg2.ShowAsync();
                        pin.latitude = -37.818514;
                        pin.longitude = 144.967855;
                        pin.pinTitle = "Melbourne";

                        break;

                    case GeolocationAccessStatus.Unspecified:
                        var msg3 = new MessageDialog("Access to the user location is.. " + accessStatus.ToString() + ". So Melbourne City is taken as the starting location");
                        await msg3.ShowAsync();
                        pin.latitude = -37.818514;
                        pin.longitude = 144.967855;
                        pin.pinTitle = "Melbourne";
                        break;

                }
               
            }
            else
            {
                var msg2 = new MessageDialog("User location not available internet connection not available. So Melbourne City is taken as the starting location");

                await msg2.ShowAsync();
                pin.latitude = -37.818514;
                pin.longitude = 144.967855;
                pin.pinTitle = "Melbourne";

            }
            return pin;
        }

        // The following  method is the critical methods which adds the pins to the map assign the icons labels and icons. 
        //It also adds the pins added to map to the global CurrentPins list so that they can referenced later.
        private async void AddPin(PinDropped tpin) 
        {
            if (tpin.latitude !=0 && tpin.longitude!= 0)
            {
                try
                {
                    Boolean pinExists = false;
                    foreach (PinDropped pinIntheList in currentPins)
                    {
                        if ((pinIntheList.latitude == tpin.latitude) && (pinIntheList.longitude == tpin.longitude))
                        {
                            pinExists = true;
                        }
                    }

                    if (!pinExists)
                    {

                        try
                        {

                            BasicGeoposition snPosition = new BasicGeoposition() { Latitude = tpin.latitude, Longitude = tpin.longitude };
                            Geopoint snPoint = new Geopoint(snPosition);
                            MapIcon mapIcon1 = new MapIcon();


                            mapIcon1.Image = RandomAccessStreamReference.CreateFromUri(
                                             new Uri("ms-appx:///Assets/pushpin.png"));

                            mapIcon1.Location = snPoint;
                            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);

                            if (currentPins.Count == 0)
                            {
                                mapIcon1.Title = pinButtonletter[0].ToString() + "\n" + tpin.pinTitle;


                            }
                            else if (currentPins.Count <= 26)
                            {

                                mapIcon1.Title = pinButtonletter[currentPins.Count].ToString() + "\n" + tpin.pinTitle;

                            }
                            else if (currentPins.Count > 26)
                            {
                                mapIcon1.Title = currentPins.Count.ToString() + "\n" + tpin.pinTitle;
                            }
                            mapIcon1.ZIndex = 0;
                            MelbMap.MapElements.Add(mapIcon1);
                            iconList.Add(mapIcon1);
                            currentPins.Add(tpin);
                        }
                        catch (Exception exp)
                        {
                            var expMsg = new MessageDialog(exp.ToString());
                            await expMsg.ShowAsync();
                        }
                    }
                    else
                    {
                        var dragMsg = new MessageDialog("Location Already on Map");
                        await dragMsg.ShowAsync();
                    }
                    getCentre(tpin.latitude, tpin.longitude, 25000);
                }
                catch (Exception exp)
                {
                    var expMsg = new MessageDialog(exp.ToString());
                    await expMsg.ShowAsync();
                }
            }
            else
            {
                var invalidPinMsg = new MessageDialog("Invalid pin");
                await invalidPinMsg.ShowAsync();
            }
        }



        // The following method uses the currentPins List to calculate the distance between them.
        //This methos also evaluvates the directions between the pins and adds them to the route and distance list which then is used 
        //to display the directions in the page.
        public async Task<List<string>> getDistanceAndRoute()

        {

            string distanceString="Distance could not be calculated";
            double distance = 0;
            List<string> routeAndDistance = new List<string>();

            if (currentPins.Count > 1)
            {
                try
                {
                    int counter = 0;
                    //int initial = 0;
                    //int final = 1;
                    string pinTitle;
                   
                    bool routeSucess;
                    if (currentPins[0].pinTitle != null)
                    {
                        routeAndDistance.Add("Starting at " + currentPins[0].pinTitle);
                    }

                    while (counter < currentPins.Count)
                    {
                        if (counter < currentPins.Count - 1)
                        {
                            BasicGeoposition startLocation = new BasicGeoposition() { Latitude = currentPins[counter].latitude, Longitude = currentPins[counter].longitude };


                            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = currentPins[counter + 1].latitude, Longitude = currentPins[counter + 1].longitude };

                            // Get the route between the points.
                            MapRouteFinderResult routeResult =
                                  await MapRouteFinder.GetDrivingRouteAsync(
                                  new Geopoint(startLocation),
                                  new Geopoint(endLocation),
                                  MapRouteOptimization.Time,
                                  MapRouteRestrictions.None);


                            if (routeResult.Status == MapRouteFinderStatus.Success)
                            {

                                // Display summary info about the route.



                                distance = distance + routeResult.Route.LengthInMeters / 1000;
                                distanceString = "Total distance = "  + distance+ "Kms";
                                routeSucess = true;




                                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                                {
                                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                                    {
                                        if ((maneuver.InstructionText).Contains("destination"))
                                        {
                                            //routeAndDistance.Add(maneuver.InstructionText);
                                            if (currentPins[counter + 1].pinTitle == currentPins[currentPins.Count - 1].pinTitle)
                                            {
                                                routeAndDistance.Add("Reached Final destination  " + currentPins[currentPins.Count - 1].pinTitle);
                                            }
                                            else
                                            {
                                                routeAndDistance.Add("reached  destination " + pinButtonletter[counter + 1] + " Location " + currentPins[counter + 1].pinTitle);
                                            }
                                        }
                                        else
                                        {
                                            routeAndDistance.Add(maneuver.InstructionText);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                routeSucess = false;
                               if(!isInternetConnected())
                                {
                                    var msg = new MessageDialog("Route could not be calculated.Please check your internet connection");
                                    await msg.ShowAsync();
                                }
                                distanceString = "Sorry Route and distance could not  be calculated. Please check your internet connection";
                                break;
                                
                            }

                        }
                            
                            counter = counter + 1;

                        }

                    }
            catch (Exception exp)
                {
                    var expMsg = new MessageDialog(exp.ToString());
                    await expMsg.ShowAsync();
                }


                routeAndDistance.Add(distanceString);
                return (routeAndDistance);
            }
            

        
                else
                {
                    routeAndDistance.Add("There is no destination");
                    return (routeAndDistance);
                }
           
        }
        //The following method generates the route legs  between the pins and draws the legs on the map.This method also calls the makeDirectionspanel which  writes the directions to the page

        //This method also calls the getDistance method which access the CurrentPins List to calculate the distance and directions
        public async Task getDirectionsonMapandPage()
        {
           
            try
            {
                if (currentPins.Count > 1)
                {
                    int counter = 0;



                    while (counter < currentPins.Count)
                    {

                        BasicGeoposition startLocation = new BasicGeoposition() { Latitude = currentPins[counter].latitude, Longitude = currentPins[counter].longitude };

                        if (counter < currentPins.Count - 1)
                        {
                            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = currentPins[counter + 1].latitude, Longitude = currentPins[counter + 1].longitude };


                            // Get the route between the points.
                            MapRouteFinderResult routeResult =
                                  await MapRouteFinder.GetDrivingRouteAsync(
                                  new Geopoint(startLocation),
                                  new Geopoint(endLocation),
                                  MapRouteOptimization.Time,
                                  MapRouteRestrictions.None);

                            if (routeResult.Status == MapRouteFinderStatus.Success)
                            {
                                // Use the route to initialize a MapRouteView.
                                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                                viewOfRoute.RouteColor = Colors.Yellow;
                                viewOfRoute.OutlineColor = Colors.Black;
                                routesList.Add(viewOfRoute);

                                // Add the new MapRouteView to the Routes collection
                                // of the MapControl.
                                MelbMap.Routes.Add(viewOfRoute);

                                // Fit the MapControl to the route.
                                await MelbMap.TrySetViewBoundsAsync(
                                      routeResult.Route.BoundingBox,
                                      null, 
                                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.Bow);
                            }
                            else
                            {
                                
                                
                                break;
                            }


                        }
                        counter = counter + 1;
                    }
                }

                List<string> routeInfoList = await getDistanceAndRoute(); //The route and distance method is called and the result is passed to makedirectionPanel method to
                //display the information


                makedirectionsPnael(routeInfoList);
            }
            catch(Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
               await expMsg.ShowAsync();
            }
        }
        // The following method is used to disply the directions information in the page
        public async void makedirectionsPnael(List<string> routeInfo)
        {
            int counter = 0;
            try
            {
                if (panelLowerDirections.Children.Count > 0)
                { panelLowerDirections.Children.Clear(); }
            }
            catch (Exception exp)
            {
                var msg = new MessageDialog(exp.ToString());
                await msg.ShowAsync();
            }
            try
            {
                foreach (string routeleg in routeInfo)
                {


                    StackPanel pnlDirectionText = new StackPanel();
                    StackPanel pnlInnerDirection = new StackPanel();
                    pnlDirectionText.Height = 25;
                    
                    pnlInnerDirection.Width = 500;
                    pnlDirectionText.Width = 500;
                    pnlInnerDirection.Orientation = Orientation.Horizontal;
                    TextBlock tblDirection = new TextBlock();
                    tblDirection.Width = 500;
                    tblDirection.TextWrapping = TextWrapping.WrapWholeWords;
                    if (routeleg != routeInfo[routeInfo.Count - 1])
                    {
                        tblDirection.Text = routeleg;
                    }
                    else
                    {
                        tblDirection.Text =  routeleg ;
                    }
                    pnlDirectionText.Children.Add(tblDirection);
                    pnlInnerDirection.Children.Add(pnlDirectionText);
                    panelLowerDirections.Children.Add(pnlInnerDirection);
                    if ((counter % 2) == 0)
                    {
                        pnlDirectionText.Background = new SolidColorBrush(Colors.Olive);
                    }
                    else
                    {
                        pnlDirectionText.Background = new SolidColorBrush(Colors.Orange);
                    }

                    counter++;


                }
            }
            catch(Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }
        }

        // The following method would be triggered when the map is tapped.The method gets the geoPosition of the tap and genrates PinDropped
        // object and passes the object to the AddPin method which in turn add the pin to the map
        private async void MelbMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            try
            {
                if(!isInternetConnected())
                {
                    var msg = new MessageDialog("For accurate locations please turn on internet connection");
                    await msg.ShowAsync();
                }
                Double lat, longi;

                var tappedGeoPosition = args.Location.Position;
                lat = tappedGeoPosition.Latitude;
                longi = tappedGeoPosition.Longitude;

                PinDropped pin = new PinDropped();
                pin.latitude = lat;
                pin.longitude = longi;
                pin.pinTitle = await getAddressFromLocation(lat, longi);
                if ( await getAddressFromLocation(lat, longi) !=null )
                {
                    AddPin(pin);
                }
                else 
                {
                    var locationMsg = new MessageDialog("Unknown location cannot add pin");
                    await locationMsg.ShowAsync();
                    //await IsLocationServiceOn();
                }
            }
            catch (Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }


        }
        

       
        private async void btnGetDirections_Click(object sender, RoutedEventArgs e)

        {
            if (!isInternetConnected())
            {
                var msg = new MessageDialog("For accurate locations and directions please turn on internet connection");
                await msg.ShowAsync();
            }
            await getDirectionsonMapandPage();

        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {


            clearMap();

        }

      

        // The following event would fire when the page is loaded.
        //This method displays all the locations in the database in the page 
        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //The data in the database is populated into the list
            var locationlist = App.conn.Table<Location>();

            //For each of the locations in the database a StackPanel named pnlInnerLocation is allocated and in that panel a Image generally a logo for that location, 
            //Name of the location in a textblock and 
            //Category of the Location in text.
            //pnlInnerLocation is also added with DragStarting delegte event 

            foreach (var dblocation in locationlist)
            {
                StackPanel pnlInnerLocation = new StackPanel();
                pnlInnerLocation.Name = dblocation.locationName;

                StackPanel pnlLocName = new StackPanel();
                pnlLocName.Width = 200;

                StackPanel pnlCategory = new StackPanel();

                pnlCategory.Width = 150;
                pnlCategory.Background = new SolidColorBrush(Colors.OrangeRed);
                ToolTipService.SetToolTip(pnlInnerLocation, "Drag on to the map");

                pnlInnerLocation.Orientation = Orientation.Horizontal;
                pnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);
                pnlInnerLocation.Margin = new Thickness(7);
                pnlInnerLocation.CanDrag = true;

                Image imgLocation = new Image();
                imgLocation.Margin = new Thickness(5, 5, 5, 5);
                imgLocation.Height = 25;
                imgLocation.Width = 20;
                imgLocation.Source = new BitmapImage(
         new Uri("ms-appx:///Assets/" + dblocation.smallimage, UriKind.Absolute));

                TextBlock tblLocation = new TextBlock();
                TextBlock tblCategory = new TextBlock();

                tblLocation.Margin = new Thickness(5, 5, 5, 5);
                tblLocation.Text = dblocation.locationName;

                tblCategory.Text = " in " + dblocation.category;
                tblCategory.Margin = new Thickness(5);

                tblCategory.Margin = new Thickness(5);
                int locationId = dblocation.locationId;

                pnlCategory.Children.Add(tblCategory);
                pnlLocName.Children.Add(tblLocation);

                pnlInnerLocation.Children.Add(imgLocation);
                pnlInnerLocation.Children.Add(pnlLocName);
                pnlInnerLocation.Children.Add(pnlCategory);
                pnlInnerLocation.AllowDrop = false;
                panelLocations.Children.Add(pnlInnerLocation);

                if (draggedPanelsList.Contains(pnlInnerLocation))
                {
                    pnlInnerLocation.CanDrag = false;

                }
                else
                {
                    pnlInnerLocation.CanDrag = true;
                }

                pnlInnerLocation.DragStarting += async delegate
                {
                    // When the location panel is dragged the address of the location is added to global label with respect to the page
                    // and the stack panel which is dragged is added to the draggedPanels list.

                    try
                    {
                        address = dblocation.address;

                        draggedPanelsList.Add(pnlInnerLocation);

                    }
                    catch (Exception exp)
                    {
                        var expMsg = new MessageDialog(exp.ToString());
                        await expMsg.ShowAsync();
                    }

                };

            }

            MelbMap.DragEnter += async delegate
            {
                // Once the the dragging of the location stack panel enter the Map the following code executes
                //PinDropped destiNationPin = await getDestinationPin(address);
                try
                {
                    if (await getDestinationPin(address) != null)//This if condition is to make sure there are  no null pins in the list
                    {
                        AddPin(await getDestinationPin(address));

                        foreach (StackPanel draggedPanel in draggedPanelsList)
                        {
                            draggedPanel.CanDrag = false;

                            ToolTipService.SetToolTip(draggedPanel, "Added to the map");
                        }
                    }
                    else
                    {
                        
                        if (!isInternetConnected())
                        {
                            address = null;
                            var msg = new MessageDialog("Location could not be added.Please check your internet connection");
                            await msg.ShowAsync();
                        }
                    }
                } 
                    catch (Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }

        };


        }



        private void MelbMap_DragOver(object sender, DragEventArgs e)
        {
            // When the drag done onto the map the following actions are taken to.
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
            e.DragUIOverride.Caption = "AddPin";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;



        }

      //The following method is used to generate address from a given latitude and longitude
        public async Task<string> getAddressFromLocation(Double latitude, Double longitude)
        {
            try
            {
                BasicGeoposition location = new BasicGeoposition();
                location.Latitude = latitude;
                location.Longitude = longitude;
                Geopoint locationGeopoint = new Geopoint(location);
                MapLocationFinderResult locationResult = await MapLocationFinder.FindLocationsAtAsync(locationGeopoint);
                if (locationResult.Status == MapLocationFinderStatus.Success)
                {
                    if (locationResult.Locations.Count > 0)
                    {
                        return locationResult.Locations[0].Address.StreetNumber+" "+ locationResult.Locations[0].Address.Street + " " + locationResult.Locations[0].Address.Town + " " + locationResult.Locations[0].Address.Region;
                    }
                    else
                    {
                        return "null";
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }
            return null;
            }
            
        


    }
}
