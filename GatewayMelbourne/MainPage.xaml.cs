using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GatewayMelbourne
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
      MessageDialog MsgForFav;
        public MainPage()
        {
            this.InitializeComponent();
            MsgForFav = new MessageDialog("Do you want save the favourites");
          
          
           MsgForFav.Commands.Add(new UICommand(
            "Save List",
            new UICommandInvokedHandler(this.CommandInvokedHandler)));
            MsgForFav.Commands.Add(new UICommand(
    "Clear List",
    new UICommandInvokedHandler(this.CommandInvokedHandler)));

            MsgForFav.Commands.Add(new UICommand(
    "Cancel",
    new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            MsgForFav.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            MsgForFav.CancelCommandIndex = 1;
            MainFrame.Navigate(typeof(MelbourneMain));
        }

        public static explicit operator MainPage(string v)
        {
            throw new NotImplementedException();
        }

        private void HamBurger_Click(object sender, RoutedEventArgs e)
        {
            MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
        }

        private async void tbCategories_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.isFavoutiteListSaved)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else if (!App.isFavoutiteListSaved)
            {
         
                await MsgForFav.ShowAsync();


            }
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            //var msg1 = new MessageDialog("The '" + command.Label + "' command has been selected.") ;
            //await msg1.ShowAsync();
            int i = 0;
            MessageDialog msg;
            switch (command.Label)
            {
                case "Save List":
                    foreach (Location favLocation in App.tempFaavouriteList)
                    {
                        favLocation.favourite = "true";
                        App.conn.Update(favLocation);
                        i++;

                    }
                    App.isFavoutiteListSaved = true;
                    msg= new MessageDialog("no of items updated" + i);
                    await msg.ShowAsync();
                    break;
                case "Cancel":
                    break;
                case "Clear List":

                    App.tempFaavouriteList.Clear();
              
                    msg = new MessageDialog("Favourite List Cleared");
                    await msg.ShowAsync();
                    App.isFavoutiteListSaved = true;
                    MainFrame.Navigate(typeof(Favourites));
                    break;

                }
        }

        private void lbCategorySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbItem_ViewFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(DayTours));

            }
            if(lbItem_AddFavourite.IsSelected)
            {
               MainFrame.Navigate(typeof(Events));
            }
            if(lbItem_DeleteFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(InTheCity));
            }
            if(lbItem_Parks.IsSelected)
            {
                MainFrame.Navigate(typeof(ParksAndGarderns));
            }
        }



        private async void tb_directions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(GetDirections));
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }

        private void asbSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            
        }

        private async void asbSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(SearchResults), args.QueryText);
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }

        private async void hlinkFav_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.isFavoutiteListSaved)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void frameGrid_Loading(FrameworkElement sender, object args)
        {
            
        }

        private async void tbHome_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(MelbourneMain));
            }
            else if (!App.isFavoutiteListSaved)
            {
               
                try {
                    await MsgForFav.ShowAsync();
                } 
                catch(Exception exp)
                {
                    var msg = new MessageDialog(exp.ToString());
                    await msg.ShowAsync();
                }
                }
        }

        private async void btnBack_Click(object sender, RoutedEventArgs e)
        {

            if (App.isFavoutiteListSaved)
            {
                if (MainFrame.CanGoBack)
                {
                    MainFrame.GoBack();
                }
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }

        private async void btnForward_Click(object sender, RoutedEventArgs e)
        {

            if (App.isFavoutiteListSaved)
            {
                if (MainFrame.CanGoForward)
                {
                    MainFrame.GoForward();
                }
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            
        }



        private void MainFrame_LayoutUpdated(object sender, object e)
        {
            // The following code is used to update the visibility of the back and forward buttons
            if (MainFrame.CanGoBack)
            {
                btnBack.IsEnabled = true;
            }
            else
            {
                btnBack.IsEnabled = false;
            }
            if (MainFrame.CanGoForward)
            {
                btnForward.IsEnabled = true;
            }
            else
            {
                btnForward.IsEnabled = false;
            }
        }

      

        private void tblDayToursCategory_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DayTours));
        }

        private void tblEventsCategory_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Events));
        }

      

        private void tblParksCategory_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(ParksAndGarderns));
        }

        private void tblIntheCity_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(InTheCity));
        }

        private void lbFavourite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbItem_ViewFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(ViewFavourites));

            }
            if (lbItem_AddFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(Favourites));
            }
            if (lbItem_DeleteFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(DeleteFavourites));
            }
          
        }

        private void tblViewFavourites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(ViewFavourites));
        }

        private void tblFavourites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Favourites));
        }

        private void tblDeleteFavourites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DeleteFavourites));
        }

        private void tbFavouritesIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private async void tblHelp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(HelpPage));
            }
            else
            {
                await MsgForFav.ShowAsync();
            }

        }
    }
}
