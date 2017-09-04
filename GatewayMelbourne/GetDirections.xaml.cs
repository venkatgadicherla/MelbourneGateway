using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        public string address="Melbourne";
        char[] pinButtonletter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public List<PinDropped> currentPins = new List<PinDropped>();
        public List<MapRouteView> routesList = new List<MapRouteView>();
        public List<MapIcon> iconList = new List<MapIcon>();
        public List<StackPanel> draggedPnaels = new List<StackPanel>();
       


        int countPins = 0;
        public GetDirections()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                clearMap();
                base.OnNavigatedTo(e);
                if (e.Parameter != null)
                {


                    if ((await getUserLocation() != null) && (await getDestinationPin(e.Parameter.ToString()) != null))
                    {
                        AddPin(await getUserLocation());
                        AddPin(await getDestinationPin(e.Parameter.ToString()));
                        await getDirections();

                    }

                }
            }
            catch(Exception exp)
            {
                var msg = new MessageDialog(exp.ToString());
                await msg.ShowAsync();
            }



        }
        public void clearMap()
        {

            int routeCounter = 0;
            while (routeCounter < routesList.Count)
            {
                MelbMap.Routes.Remove(routesList[routeCounter]);
                routeCounter++;
                currentPins.Clear();
            }
            int iconCounter = 0;
            while (iconCounter < iconList.Count)
            {
                MelbMap.MapElements.Remove(iconList[iconCounter]);


                iconCounter++;
            }
            currentPins.Clear();
            foreach (StackPanel pnl in draggedPnaels)
            {
                pnl.CanDrag = true;
               
                ToolTipService.SetToolTip(pnl, "Drag on to the map");
                
            }
            draggedPnaels.Clear();
            panelLowerDirections.Children.Clear();
           

           // tblDirections.Text = "";

        }
      

        // The following method would generate the pin for the destination when location address or name is given
        private async Task<PinDropped> getDestinationPin(string locationName)
        {
            PinDropped destinationPin = new PinDropped();
            BasicGeoposition melbourne = new BasicGeoposition();
            melbourne.Latitude = -37.8136;
            melbourne.Longitude = 144.9631;
            Geopoint hintpoint = new Geopoint(melbourne);
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(locationName, hintpoint);
            if (result.Status == MapLocationFinderStatus.Success)
            {
                
                destinationPin.latitude = result.Locations[0].Point.Position.Latitude;
                destinationPin.longitude = result.Locations[0].Point.Position.Longitude;
                destinationPin.pinTitle = locationName;

            }
            else
            {
                var msg1 = new MessageDialog(result.Status.ToString());
                await msg1.ShowAsync();
            }
            return destinationPin;
        }


        public class PinDropped
        {
            public double latitude { get; set; }
            public double longitude { get; set; }

            public string pinTitle { get; set; }

        }

       //The following event when the map is loaded. In this method the City of Melbourne is made the centre of the map.
       public async void getCentre(double latitude,double longitude,int zoom)
        {

            var center =
            new Geopoint(new BasicGeoposition()
            {
                Latitude = latitude,

               Longitude = longitude

            });
            await MelbMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, zoom));
        }
        private async void MelbMap_Loaded(object sender, RoutedEventArgs e)
        {

          

            getCentre(-37.818514, 144.967855,25000);
        }

        // The following method is used to get the location of the user based on their location. 
        public async Task<PinDropped> getUserLocation()
        {
            PinDropped pin = new PinDropped();
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    var geoLocator = new Geolocator();
                    geoLocator.DesiredAccuracy = PositionAccuracy.High;
                    Geoposition pos = await geoLocator.GetGeopositionAsync();
                    Double latitude = Convert.ToDouble( pos.Coordinate.Point.Position.Latitude);
                    Double longitude = Convert.ToDouble(pos.Coordinate.Point.Position.Longitude); ;
                    pin.latitude = latitude;
                    pin.longitude = longitude;
                    pin.pinTitle = await getAddressFromLocation(latitude,longitude);

                    break;

                case GeolocationAccessStatus.Denied: 
                    var msg2 = new MessageDialog("Access to the user location is "+ accessStatus.ToString()+". So Melbourne City is taken as the starting location");

                    await msg2.ShowAsync();
                    pin.latitude = -37.818514;
                    pin.longitude = 144.967855;
                    pin.pinTitle = "Melbourne";
                    
                    break;

                case GeolocationAccessStatus.Unspecified:
                    var msg3 = new MessageDialog("Access to the user location is "+ accessStatus.ToString() + ". So Melbourne City is taken as the starting location");
                    await msg3.ShowAsync();
                    pin.latitude = -37.818514;
                    pin.longitude = 144.967855;
                    pin.pinTitle = "Melbourne";
                    break;

            }
            return pin;
        }

        private async void AddPin(PinDropped tpin)
        {
            Boolean pinExists=false;
            foreach(PinDropped pinIntheList in currentPins)
            {
               if( (pinIntheList.latitude==tpin.latitude) && (pinIntheList.longitude==tpin.longitude))
                    {
                    pinExists = true;
                }
            }

            if (!pinExists)
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
                    mapIcon1.Title = pinButtonletter[0].ToString() +"\n"+ tpin.pinTitle;
                 

                }
                else if (currentPins.Count < 26)
                {

                    mapIcon1.Title = pinButtonletter[currentPins.Count].ToString()+ "\n" +tpin.pinTitle;
                   
                }
                else if (currentPins.Count >= 26)
                {
                    mapIcon1.Title = currentPins.Count.ToString()+"\n" + tpin.pinTitle;
                }
                mapIcon1.ZIndex = 0;
                MelbMap.MapElements.Add(mapIcon1);
                iconList.Add(mapIcon1);
                currentPins.Add(tpin);
            }
            else
            {
                var dragMsg = new MessageDialog("Location Already on Map");
                await dragMsg.ShowAsync();
            }
            getCentre(tpin.latitude, tpin.longitude,25000);
        }
       
        private async void MelbMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            Double lat, longi;




            var tappedGeoPosition = args.Location.Position;
            lat = tappedGeoPosition.Latitude;
            longi = tappedGeoPosition.Longitude;

            PinDropped pin = new PinDropped();
            pin.latitude = lat;
            pin.longitude = longi;
            pin.pinTitle = await getAddressFromLocation(lat, longi);

            AddPin(pin);


        }

        public async Task <List<string>> getDistance1()

        {
            double distance = 0;
            String routeInfo = "";
            
            List<string> routeAndDistance = new List<string>();
            if (currentPins.Count > 1)
            {
                int counter = 0;
                //int initial = 0;
                //int final = 1;
                routeAndDistance.Add("Starting at "+currentPins[0].pinTitle);

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
                        }
                        else
                        {
                            distance = 1000000;
                        }
                        foreach (MapRouteLeg leg in routeResult.Route.Legs)
                        {
                            foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                            {
                                if ((maneuver.InstructionText).Contains("destination"))
                                {
                                    //routeAndDistance.Add(maneuver.InstructionText);
                                    if (currentPins[counter + 1].pinTitle== currentPins[currentPins.Count - 1].pinTitle)
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
                    counter = counter + 1;

                }


               
                routeAndDistance.Add(distance.ToString());
                return (routeAndDistance);
            }
            else
            {
                routeAndDistance.Add("There is no destination");
                return (routeAndDistance);
            }
        }
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
            foreach (string routeleg in routeInfo)
            {


                StackPanel pnlDirectionText = new StackPanel();
                StackPanel pnlInnerDirection = new StackPanel();
                pnlInnerDirection.Width = 500;
                pnlDirectionText.Width = 500;
                pnlInnerDirection.Orientation = Orientation.Horizontal;
                TextBlock tblDirection = new TextBlock();
                tblDirection.Width = 500;
                if (routeleg != routeInfo[routeInfo.Count - 1])
                {
                    tblDirection.Text = routeleg;
                }
                else
                {
                    tblDirection.Text = "Total distance =" + routeleg + " Kms";
                }
                pnlDirectionText.Children.Add(tblDirection);
                pnlInnerDirection.Children.Add(pnlDirectionText);
                panelLowerDirections.Children.Add(pnlInnerDirection);
                if ((counter % 2) == 0)
                {
                    pnlDirectionText.Background = new SolidColorBrush(Colors.LightSkyBlue);
                }
                else
                {
                    pnlDirectionText.Background = new SolidColorBrush(Colors.DeepSkyBlue);
                }

                counter++;


            }
        }
        public async Task getDirections()
        {
            
            var messageDialog = new MessageDialog(" The no of pins dropped are " + currentPins.Count);
            await messageDialog.ShowAsync();


            if (currentPins.Count > 1)
            {
                int counter = 0;



                while (counter < currentPins.Count)
                {

                    BasicGeoposition startLocation = new BasicGeoposition() { Latitude = currentPins[counter].latitude, Longitude = currentPins[counter].longitude };

                    // End at the city of Seattle, Washington.
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


                    }
                    counter = counter + 1;
                }
            }
            //tblDirections.Text = "";
            List<string> routeInfoList = await getDistance1();
            
          
            makedirectionsPnael(routeInfoList);
        }

        private void MelbMap_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {


        }

        private async void btnGetDirections_Click(object sender, RoutedEventArgs e)
        {
            await getDirections();
          


        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {


            clearMap();

        }

        private void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            getUserLocation();
        }

        // The following event would fire when the page is loaded. 
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            var locationlist = App.conn.Table<Location>();
          
            //tooltip.Content = "Drag on to the map";
            //tooltip.Background = new SolidColorBrush(Colors.IndianRed);
        
            foreach (var dblocation in locationlist)
            {
                StackPanel pnlInnerLocation = new StackPanel();
                pnlInnerLocation.Name = dblocation.locationName;
                StackPanel pnlImagelocation = new StackPanel();
                pnlImagelocation.Width = 200;
                StackPanel pnlCategory = new StackPanel();
                pnlCategory.Width = 150;
                pnlCategory.Background = new SolidColorBrush(Colors.OrangeRed);
                ToolTipService.SetToolTip(pnlInnerLocation, "Drag on to the map");
                pnlInnerLocation.Orientation = Orientation.Horizontal;
                pnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);

                //string address = "Melbourne";


                pnlInnerLocation.Margin = new Thickness(7);
                pnlInnerLocation.CanDrag = true;


              
                Image imgLocation = new Image();
                imgLocation.Margin = new Thickness(5, 5, 5, 5);
                imgLocation.Height = 25;
                imgLocation.Width = 20;
                imgLocation.Source = new BitmapImage(
         new Uri("ms-appx:///Assets/" + dblocation.smallimage, UriKind.Absolute));

                TextBlock tblImageDescription = new TextBlock();
                TextBlock tblImageCategory = new TextBlock();

                tblImageDescription.Margin = new Thickness(5, 5, 5, 5);
                tblImageDescription.Text = dblocation.locationName;
                tblImageCategory.Text = " in " + dblocation.category;
                tblImageCategory.Margin = new Thickness(5);

                tblImageCategory.Margin = new Thickness(5);
                int locationId = dblocation.locationId;

                pnlCategory.Children.Add(tblImageCategory);
                pnlImagelocation.Children.Add(tblImageDescription);

                pnlInnerLocation.Children.Add(imgLocation);
                pnlInnerLocation.Children.Add(pnlImagelocation);
                pnlInnerLocation.Children.Add(pnlCategory);
                pnlInnerLocation.AllowDrop = false;
                panelLocations.Children.Add(pnlInnerLocation);
                if (draggedPnaels.Contains(pnlInnerLocation))
                {
                    pnlInnerLocation.CanDrag = false;
                  
                }
                else
                {
                    pnlInnerLocation.CanDrag = true;
                }

                pnlInnerLocation.DragStarting += delegate
                {
                    address = dblocation.address;
                   
                    draggedPnaels.Add(pnlInnerLocation);
                   


                };
              
                }

            MelbMap.DragEnter += async delegate
            {
                AddPin(await getDestinationPin(address));
              foreach(StackPanel draggedPanel in draggedPnaels)
                {
                    draggedPanel.CanDrag = false;
                                            
                    ToolTipService.SetToolTip(draggedPanel, "Added to the map");
                }
            };


        }

       

        private void MelbMap_DragOver(object sender, DragEventArgs e)
        {

            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
            e.DragUIOverride.Caption = "AddPin";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;



        }
        public async Task<string> getAddressFromLocation(Double latitude, Double longitude)
        {
            BasicGeoposition location = new BasicGeoposition();
            location.Latitude = latitude;
            location.Longitude = longitude;
            Geopoint locationGeopoint = new Geopoint(location);
            MapLocationFinderResult locationResult = await MapLocationFinder.FindLocationsAtAsync(locationGeopoint);
            if(locationResult.Status==MapLocationFinderStatus.Success)
            {
                return locationResult.Locations[0].Address.StreetNumber + locationResult.Locations[0].Address.Street + locationResult.Locations[0].Address.Town + locationResult.Locations[0].Address.Region;
            }
            return " ";
        }


    }
}
