using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class SearchResults : Page
    {


        public SearchResults()
        {
            this.InitializeComponent();
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        { }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //The OnNavigatedTo event occurs when the app navigates to the SearchResults page.
            //In this page  the the description of the locations is compared with the search string and results are displayed 
            //appropriately.
            try
            {
                base.OnNavigatedTo(e);
                string lookUp = e.Parameter.ToString();
                if (!String.IsNullOrWhiteSpace(lookUp))
                {
                    string normalisedLookup = Regex.Replace(lookUp, @"\s", "").ToLower();
                    SearchObjects SearchResultobj = new SearchObjects();
                    SearchResultobj.Search(normalisedLookup);
                    List<Location> searchResult = SearchResultobj.getsearchResult();
                    tbsearchResults.Text += "\nYour Search for " + lookUp.ToString() + " resulted in " + searchResult.Count + " items";

                    if (searchResult.Count > 0)
                    {
                        foreach (Location tempLocation in searchResult)
                        {
                            StackPanel insideStack = new StackPanel();
                            insideStack.Name = "insideStackName";
                            insideStack.Orientation = Orientation.Horizontal;
                            StackPanel innerInsideStack = new StackPanel();

                            //innerInsideStack.VerticalAlignment = VerticalAlignment.Bottom;
                            innerInsideStack.Name = "innerInsideStack";
                            innerInsideStack.VerticalAlignment = VerticalAlignment.Top;
                            innerInsideStack.Margin = new Thickness(3);

                            HyperlinkButton hyperLinkButton = new HyperlinkButton();
                            Image imgLocation = new Image();
                            imgLocation.Margin = new Thickness(0);
                            imgLocation.Height = 25;
                            imgLocation.Width = 20;
                            imgLocation.Source = new BitmapImage(
                     new Uri("ms-appx:///Assets/" + tempLocation.smallimage, UriKind.Absolute));
                            TextBlock tbSearchItemDesc = new TextBlock();
                            hyperLinkButton.Foreground = new SolidColorBrush(Colors.Black);

                            hyperLinkButton.VerticalAlignment = VerticalAlignment.Bottom;
                            hyperLinkButton.Content = tempLocation.locationName;

                            tbSearchItemDesc.Text = tempLocation.description;
                            tbSearchItemDesc.TextWrapping = TextWrapping.WrapWholeWords;
                            tbSearchItemDesc.FontSize = 12;
                            tbSearchItemDesc.VerticalAlignment = VerticalAlignment.Bottom;
                            string navigateControl = tempLocation.page;
                            hyperLinkButton.Click += delegate

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

                                    //End App pages

                            }

                            };
                            insideStack.Children.Add(imgLocation);
                            innerInsideStack.Children.Add(hyperLinkButton);
                            innerInsideStack.Children.Add(tbSearchItemDesc);
                            insideStack.Children.Add(innerInsideStack);
                            panelSearchResults.Children.Add(insideStack);

                        }

                    }
                }
                else { tbsearchResults.Text += "\nYour Search resulted in zero items"; }
            }
            catch (Exception exp)
            {
                var expMsg = new MessageDialog(exp.ToString());
                await expMsg.ShowAsync();
            }

        }
    }
}
