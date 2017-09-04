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
        

        



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.isFavouritePage = true;
            var locationlist = App.conn.Table<Location>();
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

                if (dblocation.favourite.ToLower().CompareTo("false") == 0 )
                {
                    pnlInnerLocation.CanDrag = true;
                   

                }
                if (dblocation.favourite.ToLower().CompareTo("true") == 0)
                {
                    pnlInnerLocation.CanDrag = false;

                    ToolTipService.SetToolTip(pnlInnerLocation, toolTip);

                }
                if (App.tempFaavouriteList.Count>0)
                {
                    foreach(var tempFav in App.tempFaavouriteList)
                    {
                        if(tempFav.locationId==dblocation.locationId)
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
                pnlInnerLocation.DragStarting += delegate { draggedLocationID = locationId; 
                };
                pnlInnerLocation.DropCompleted += delegate
                {
                    if (App.tempFaavouriteList.Count > 0)
                    {
                        foreach (var tempFav in App.tempFaavouriteList)
                        {
                            if (tempFav.locationId == dblocation.locationId)
                            {
                                pnlInnerLocation.CanDrag = false;
                                //pnlInnerLocation.Tag = "Already a Favourite";
                               
                                ToolTipService.SetToolTip(pnlInnerLocation, toolTip);
                                
                            }
                        }
                    }
                };
            }
                // The following code will populate the favourites
               
                    foreach (var Fav in  locationlist)
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
                        //int favlocationId = dblocation.locationId;


                        favpnlInnerLocation.Children.Add(favimgLocation);
                        favpnlInnerLocation.Children.Add(favtblImageDescription);
                        favpnlInnerLocation.Children.Add(favtblImageCategory);
                        panelFavourite.Children.Add(favpnlInnerLocation);
                    }
                    }
                
              
            
        }

        private  void panelFavourite_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {

            
        }

        private  void panelFavourite_Drop(object sender, DragEventArgs e)
        {
            // List<Location> draggedLocation=currentLocation.retrieveLocation(draggedLocationID);

            App.isFavoutiteListSaved = false;
            var locationlist = App.conn.Table<Location>();
           
            Location draggedFavLocation=null;
            foreach (var dblocation in locationlist)
            {
                if (dblocation.locationId == draggedLocationID)
                {
                    draggedFavLocation = dblocation;
                    App.tempFaavouriteList.Add(draggedFavLocation);
                }

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
            favtblImageCategory.Text = " in "+ draggedFavLocation.category;
            favtblImageCategory.Margin = new Thickness(5);
            //int favlocationId = dblocation.locationId;


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
            var updateFav = App.conn.Table<Location>();
            int i = 0;
            
            foreach (Location favLocation in App.tempFaavouriteList)
            {
                favLocation.favourite = "true";
                App.conn.Update(favLocation);
                i++;
               
            }
            App.isFavoutiteListSaved = true;
            var msg = new MessageDialog("Favourites saved ");
            await msg.ShowAsync();
        }
    }
}
