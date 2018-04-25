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
    public sealed partial class ViewFavourites : Page
    {
        public ViewFavourites()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {// On page loading the favourites are displayed and user can click on the locattions to go to that page
            try
            {
                App.isFavouritePage = true;
                var locationlist = App.conn.Table<Location>();
                foreach (var dblocation in locationlist)
                {
                    StackPanel pnlInnerLocation = new StackPanel();
                    pnlInnerLocation.Orientation = Orientation.Horizontal;
                    pnlInnerLocation.Background = new SolidColorBrush(Colors.Orange);



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
                       

                        tblImageDescription.Margin = new Thickness(5);
                        tblImageDescription.Text = dblocation.locationName;
                        tblImageCategory.Text = " in " + dblocation.category;
                        tblImageCategory.Margin = new Thickness(5);
                        int locationId = dblocation.locationId;


                        pnlInnerLocation.Children.Add(imgLocation);
                        pnlInnerLocation.Children.Add(tblImageDescription);
                        pnlInnerLocation.Children.Add(tblImageCategory);
                     
                        panelFavourite.Children.Add(pnlInnerLocation);
                       
                        string navigateControl = dblocation.page;
                       pnlInnerLocation.Tapped += delegate
                        {
                            switch (navigateControl)
                            {

                                //In the city
                                case "itemCrown":

                                    Frame.Navigate(typeof(itemCrown));
                                    break;
                                case "itemFlindersStreet":

                                    Frame.Navigate(typeof(itemFlindersStreet));
                                    break;
                                case "itemMarket":

                                    Frame.Navigate(typeof(itemMarket));
                                    break;
                                case "itemStkilda":

                                    Frame.Navigate(typeof(itemStkilda));

                                    break;
                                //End of in the city
                                //Begin Parks gardens 
                                case "itemMelbZoo":

                                    Frame.Navigate(typeof(itemMelbZoo));
                                    break;
                                case "itemWerribeeZoo":

                                    Frame.Navigate(typeof(itemWerribeeZoo));
                                    break;
                                case "Aquarium":

                                    Frame.Navigate(typeof(Aquarium));
                                    break;
                                case "itemBotanicalGarden":

                                    Frame.Navigate(typeof(itemBotanicalGarden));
                                    break;

                                //End Parks and Gardens

                                // Begin Events
                                case "itemMelbCup":
                                    Frame.Navigate(typeof(itemMelbCup));
                                    break;
                                case "itemAfl":
                                    Frame.Navigate(typeof(itemAfl));
                                    break;
                                case "itemAusOpen":
                                    Frame.Navigate(typeof(itemAusOpen));
                                    break;
                                case "itemF1":
                                    Frame.Navigate(typeof(itemF1));
                                    break;

                                //End Events

                                //Begin Day Tours
                                case "itemGreatOceanDrive":
                                    Frame.Navigate(typeof(itemGreatOceanDrive));
                                    break;
                                case "itemMountBuller":
                                    Frame.Navigate(typeof(itemMountBuller));
                                    break;
                                case "ItemPhilipIsland":
                                    Frame.Navigate(typeof(ItemPhilipIsland));
                                    break;
                                case "YarraValley":
                                    Frame.Navigate(typeof(YarraValley));
                                    break;
                                //End Day tours

                                //Begin App pages
                                case "Directions":
                                    Frame.Navigate(typeof(GetDirections));
                                    break;

                                case "Main":

                                    Frame.Navigate(typeof(MelbourneMain));
                                    break;

                                case "Help":
                                    Frame.Navigate(typeof(HelpPage));
                                    break;



                                default:
                                    Frame.Navigate(typeof(MelbourneMain));
                                    break;

                            };
                        };

                    }

                }
            }
            catch (Exception exp)
            {
                var msgExp = new MessageDialog(exp.ToString());
                await msgExp.ShowAsync();
            }
        }
    }
}
