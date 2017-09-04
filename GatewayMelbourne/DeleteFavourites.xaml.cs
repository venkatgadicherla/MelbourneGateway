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
    public sealed partial class DeleteFavourites : Page
    {
        public DeleteFavourites()
        {
            this.InitializeComponent();
            headingPanel.Margin = new Thickness(10, 10, 10, 10);
            headingPanel.Background = new SolidColorBrush(Colors.OrangeRed);
            headingPanel.Height = 50;
            headingPanel.CornerRadius = new CornerRadius(10);
            tblFavouriteHdr.Text = "Favourites";
            tblFavouriteHdr.Margin = new Thickness(58, 10, 86, 10);
            tblFavouriteHdr.FontFamily = new FontFamily("Comic Sans Ms");
            tblFavouriteHdr.Foreground = new SolidColorBrush(Colors.White);
            tblFavouriteHdr.TextAlignment = TextAlignment.Center;
            headingPanel.Children.Add(tblFavouriteHdr);

        }
        int noOfFavourites;
        StackPanel headingPanel = new StackPanel();
        TextBlock tblFavouriteHdr = new TextBlock();
        StackPanel panelLocationToDelete = new StackPanel();
        Location deleteLocation ;
        //Location locationlist ;

        private void loadFavourite(StackPanel panelFavourite)
        {
            App.isFavouritePage = true;
            panelFavourite.AllowDrop = false;
            //StackPanel panelLocationToDelete = new StackPanel();
            //Location deleteLocation = new Location();
            var locationlist = App.conn.Table<Location>();
            noOfFavourites = 0;
            foreach (var dblocation in locationlist)
            {

                StackPanel pnlInnerLocation = new StackPanel();
                pnlInnerLocation.Orientation = Orientation.Horizontal;
                pnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);
                pnlInnerLocation.AllowDrop = false;



                pnlInnerLocation.Margin = new Thickness(10, 5, 5, 10);
                pnlInnerLocation.CornerRadius = new CornerRadius(10);
                pnlInnerLocation.CanDrag = true;


                if (dblocation.favourite.ToLower().CompareTo("true") == 0)
                {

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
                    int locationId = dblocation.locationId;


                    pnlInnerLocation.Children.Add(imgLocation);
                    pnlInnerLocation.Children.Add(tblImageDescription);
                    pnlInnerLocation.Children.Add(tblImageCategory);
                    panelFavourite.Children.Add(pnlInnerLocation);

                    // //When the location to be deleted from favourites and when it is dragged onto recycle bin Icon the following code executes
                    pnlInnerLocation.DragStarting += delegate
                    {

                        panelLocationToDelete = pnlInnerLocation;
                        deleteLocation = dblocation;
                    };

                    noOfFavourites++;


                    //tblBin.DragEnter += delegate
                    //{

                    //    if (panelFavourite.Children.Contains(panelLocationToDelete))
                    //    {
                    //        panelFavourite.Children.Remove(panelLocationToDelete);
                    //        deleteLocation.favourite = "false";
                    //        App.conn.Update(deleteLocation);
                    //    }
                    //};


                }
            }
            }

        private void Page_Loading(FrameworkElement sender, object args)
        {

              panelFavourite.Children.Add(headingPanel);

            loadFavourite(panelFavourite);
        }

        private void tbldeleteAllHeader_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var locationlist = App.conn.Table<Location>();
            noOfFavourites = 0;
            foreach (var dblocation in locationlist)
            {
                dblocation.favourite = "false";
                App.conn.Update(dblocation);
            }
            panelFavourite.Children.Clear();
            panelFavourite.Children.Add(headingPanel);
            App.tempFaavouriteList.Clear();
        }

        private  void tblBin_DragEnter(object sender, DragEventArgs e)
        {
           
            if (panelFavourite.Children.Contains(panelLocationToDelete))
            {
               
                panelFavourite.Children.Remove(panelLocationToDelete);
                deleteLocation.favourite = "false";
                App.conn.Update(deleteLocation);
               
            }
        }
    }
}

