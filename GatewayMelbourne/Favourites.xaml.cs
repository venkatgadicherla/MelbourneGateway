using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class Favourites : Page

    {
        public Favourites()
        {
            this.InitializeComponent();
            toolTip.Content = "Already a Favourite";

        }
        List<int> locationId = new List<int>();
        int draggedLocationID;
        DbHelperClass currentLocation = new DbHelperClass();
        ToolTip toolTip = new ToolTip();






        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // The following code gets the locations information from the database and populates  the locations in the page appropirately.
            // The design of a single location is as follows
            //pnlInnerLocation is the stack panel which contains three elements which are 2 stack panels and 1 image
            //1 Location image-- imgLocation
            //2 Location name--pnlActualLocation-- a stack panel--which contains a textblock-tblLocationName which contains the text for the location
            //3 Category information-pnlCategory-- a staCK panel--which contains a textblock-tblLocationCategory which contains the text for the location
            //pnlInnerLocation is object is created for each of the location in database and added to the Location panel in the page.
            //For each of the pnlInnerLocation objects two  delegate events are also attached. One dragStarting and dropCompleted.

            App.isFavouritePage = true;
            var locationlist = App.conn.Table<Location>();
            //The following code would display all the locations in the database which are favourites and not favourites and would populate the locations panel
            try
            {
                foreach (var dblocation in locationlist)
                {
                    StackPanel pnlInnerLocation = new StackPanel();
                    StackPanel pnlImagelocation = new StackPanel();
                    pnlImagelocation.Width = 200;
                    StackPanel pnlCategory = new StackPanel();
                    pnlCategory.Width = 150;
                    pnlCategory.Background = new SolidColorBrush(Colors.OrangeRed);
                    pnlInnerLocation.Orientation = Orientation.Horizontal;
                    pnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);




                    pnlInnerLocation.Margin = new Thickness(7);
                    pnlInnerLocation.CanDrag = true;

                    if (dblocation.favourite.ToLower().CompareTo("false") == 0) // if the Location is not a favourite then only user can add it to favourites. The following
                    {                                                            //code enables that logic
                        pnlInnerLocation.CanDrag = true;


                    }
                    if (dblocation.favourite.ToLower().CompareTo("true") == 0)// if the Location is a favourite the following makes sure it can not be added to the favourites
                    {
                        pnlInnerLocation.CanDrag = false;

                        ToolTipService.SetToolTip(pnlInnerLocation, toolTip);

                    }
                    if (App.tempFavouriteList.Count > 0)//If the locations are just added to the Favourites and the list is not saved 
                    {                                 //then the following code makes sure they cannot be added again

                        foreach (var tempFav in App.tempFavouriteList)
                        {
                            if (tempFav.locationId == dblocation.locationId)
                            {
                                pnlInnerLocation.CanDrag = false;
                                ToolTipService.SetToolTip(pnlInnerLocation, toolTip);
                            }
                        }
                    }
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
                    panelLocations.Children.Add(pnlInnerLocation);

                    pnlInnerLocation.DragStarting += delegate
                    {// This is delegate event for the individual Stack panel which contains the location details.
                        draggedLocationID = locationId; //The dragged locationId value which is to be added to the favourites panel and updated in the database
                                                        //added to the golbal variable  draggedLocationId. This id is later used to update the Locations details in the 
                                                        //database
                    };
                    pnlInnerLocation.DropCompleted += delegate
                    {//This event is triggered after the pnlInnerLaocation is dropped and panelFavourite_Drop event is completed.It sets Location not to be dragged and dropped
                     //again and also sets up the tool tip to indicate the favourite status of that particular Location
                        if (App.tempFavouriteList.Count > 0)
                        {
                            foreach (var tempFav in App.tempFavouriteList)
                            {
                                if (tempFav.locationId == dblocation.locationId)
                                {
                                    pnlInnerLocation.CanDrag = false;


                                    ToolTipService.SetToolTip(pnlInnerLocation, toolTip);

                                }
                            }
                        }
                    };
                }
            }
            catch (Exception exp)
            {
                var msgExp = new MessageDialog(exp.ToString());
                await msgExp.ShowAsync();
            }
            // The following code will populate the favourites panel if there are any favourites already

            foreach (var Fav in locationlist)
            {try
                {
                    if (Fav.favourite.ToLower().CompareTo("true") == 0)
                    {
                        StackPanel favpnlInnerLocation = new StackPanel();
                        favpnlInnerLocation.Orientation = Orientation.Horizontal;
                        favpnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);
                        favpnlInnerLocation.Margin = new Thickness(7);

                        Image favimgLocation = new Image();
                        favimgLocation.Margin = new Thickness(5, 5, 5, 5);
                        favimgLocation.Height = 25;
                        favimgLocation.Width = 20;
                        favimgLocation.Source = new BitmapImage(
                 new Uri("ms-appx:///Assets/" + Fav.smallimage, UriKind.Absolute));

                        TextBlock favtblImageDescription = new TextBlock();
                        TextBlock favtblImageCategory = new TextBlock();

                        favtblImageDescription.Margin = new Thickness(5, 5, 5, 5);
                        favtblImageCategory.Foreground = new SolidColorBrush(Colors.White);
                        favtblImageDescription.Text = Fav.locationName;
                        favtblImageDescription.Foreground = new SolidColorBrush(Colors.White);
                        favtblImageCategory.Text = " in " + Fav.category;
                        favtblImageCategory.Margin = new Thickness(5);



                        favpnlInnerLocation.Children.Add(favimgLocation);
                        favpnlInnerLocation.Children.Add(favtblImageDescription);
                        favpnlInnerLocation.Children.Add(favtblImageCategory);
                        panelFavourite.Children.Add(favpnlInnerLocation);
                    }
                }
                catch(Exception exp)
                {
                    var msgExp = new MessageDialog(exp.ToString());
                    await msgExp.ShowAsync();
                }
            }



        }

        private  void panelFavourite_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {

            
        }

        private async void panelFavourite_Drop(object sender, DragEventArgs e)
        {
            //This method  is triggered when a drop event happens with panelFavourite as the target.
            //In this method the App.isFavouriteListSaved is set to false to disable navigation from the before saving the list.
            //The draggedLocationId is used to display the location details
            //The details of the Location are also addded to the tempFaavouriteList to facilitate navigation and display.

            App.isFavoutiteListSaved = false;
            Location draggedFavLocation = null;
            try
            {
                var locationlist = App.conn.Table<Location>();

               
                foreach (var dblocation in locationlist)
                {
                    if (dblocation.locationId == draggedLocationID)
                    {
                        draggedFavLocation = dblocation;
                        App.tempFavouriteList.Add(draggedFavLocation);
                    }

                }
            }
            catch (Exception exp)
            {
                var msgExp = new MessageDialog(exp.ToString());
                await msgExp.ShowAsync();
            }
            StackPanel favpnlInnerLocation = new StackPanel();
            favpnlInnerLocation.Orientation = Orientation.Horizontal;
            favpnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);
            favpnlInnerLocation.Margin = new Thickness(7);

            Image favimgLocation = new Image();
            favimgLocation.Margin = new Thickness(5, 5, 5, 5);
            favimgLocation.Height = 25;
            favimgLocation.Width = 20;
            favimgLocation.Source = new BitmapImage(
     new Uri("ms-appx:///Assets/" + draggedFavLocation.smallimage, UriKind.Absolute));

            TextBlock favtblImageDescription = new TextBlock();
            TextBlock favtblImageCategory = new TextBlock();

            favtblImageDescription.Margin = new Thickness(5, 5, 5, 5);
            favtblImageCategory.Foreground = new SolidColorBrush(Colors.White);
            favtblImageDescription.Text = draggedFavLocation.locationName;
            favtblImageDescription.Foreground = new SolidColorBrush(Colors.White);
            favtblImageCategory.Text = " in " + draggedFavLocation.category;
            favtblImageCategory.Margin = new Thickness(5);



            favpnlInnerLocation.Children.Add(favimgLocation);
            favpnlInnerLocation.Children.Add(favtblImageDescription);
            favpnlInnerLocation.Children.Add(favtblImageCategory);
            panelFavourite.Children.Add(favpnlInnerLocation);


        }

        private void panelFavourite_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
            e.DragUIOverride.Caption = "Copy";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void tblSaveFavoutites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // This method sets the  Locations Favourites status  true in the database 
            try
            {
                var updateFav = App.conn.Table<Location>();
                int i = 0;

                foreach (Location favLocation in App.tempFavouriteList)
                {
                    favLocation.favourite = "true";
                    App.conn.Update(favLocation);
                    i++;

                }
                App.isFavoutiteListSaved = true;
                var msg = new MessageDialog("Favourites saved ");
                await msg.ShowAsync();
            }
            catch(Exception exp)
            {
                var msgExp = new MessageDialog(exp.ToString());
                await msgExp.ShowAsync();
            }
        }
    }
}
