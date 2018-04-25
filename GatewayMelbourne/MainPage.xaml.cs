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
        MessageDialog MsgForFav;// This Message dialog is used to direct the navigation when using the Favourites page
        public MainPage()
        {
                this.InitializeComponent();
                MainFrame.Navigate(typeof(MelbourneMain));//The app is directed to Melbourne Main page upon loading.

            //The following code is used to initialize the display of the MessageDialog MsgForFav
            MsgForFav = new MessageDialog("Do you want save the favourites");
            
                MsgForFav.Commands.Add(new UICommand("Add and Save",new UICommandInvokedHandler(this.CommandInvokedHandler)));

                MsgForFav.Commands.Add(new UICommand( "Discard changes",new UICommandInvokedHandler(this.CommandInvokedHandler)));

                MsgForFav.Commands.Add(new UICommand("Cancel",new UICommandInvokedHandler(this.CommandInvokedHandler)));

                // Set the command that will be invoked by default
                MsgForFav.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                MsgForFav.CancelCommandIndex = 1;

           
        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            //This method is used to incorporate the functionality of the different options available in the   MsgForFav MessageDialog

           
            MessageDialog msg;
            switch (command.Label)
            {
                case "Add and Save":// The following actions take place when Save List 
                    try
                    {
                        foreach (Location favLocation in App.tempFavouriteList)
                        {
                            favLocation.favourite = "true";
                            App.conn.Update(favLocation);

                        }
                        App.isFavoutiteListSaved = true;
                        msg = new MessageDialog("Favourites Saved");
                        await msg.ShowAsync();
                    }
                    catch(Exception exp)
                    {
                        var msgExp = new MessageDialog(exp.ToString());
                        await msgExp.ShowAsync();
                    }
                    break;

                case "Cancel":// The Following actions take place when Cancel option is selected

                        break;

                case "Discard changes"://The tempFavouriteList is cleared when the Clear List is selected

                    App.tempFavouriteList.Clear();

                    msg = new MessageDialog("The newly added items are discarded");
                    await msg.ShowAsync();
                    App.isFavoutiteListSaved = true;
                    MainFrame.Navigate(typeof(Favourites));
                    break;

            }
        }


        public static explicit operator MainPage(string v)
        {
            throw new NotImplementedException();
        }

        //  MainPage Page events start here
        private async void asbSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //This event occurs when Favourites text block is clicked
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(SearchResults), args.QueryText);
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }

        private void HamBurger_Click(object sender, RoutedEventArgs e)
        {
            // This event is occurs when Hamburger menu button is clicked and toggles the state of the menu
            MenuSplitView.IsPaneOpen = !MenuSplitView.IsPaneOpen;
        }
        //Main page events end here

        //Main menu Tap events
        private async void tbHome_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(MelbourneMain));
            }
            else if (!App.isFavoutiteListSaved)
            {

                try
                {
                    await MsgForFav.ShowAsync();
                }
                catch (Exception exp)
                {
                    var msg = new MessageDialog(exp.ToString());
                    await msg.ShowAsync();
                }
            }
        }
        private async void tbCategories_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //This event occurs when Categories is tapped and categories Flyout is opened
            if (App.isFavoutiteListSaved)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
            else if (!App.isFavoutiteListSaved)
            {

                await MsgForFav.ShowAsync();


            }
        }
        private async void tb_directions_Tapped(object sender, TappedRoutedEventArgs e)
          {
            // This event occurs when directions textblock is selected
            if (App.isFavoutiteListSaved)
            {
                MainFrame.Navigate(typeof(GetDirections));
            }
            else
            {
                await MsgForFav.ShowAsync();
            }
        }
        private async void tblFavourites_Tapped(object sender, TappedRoutedEventArgs e)
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
       //Main menu events here

        
      //Navigation events start from here

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

     
        //Navigation menu ends here
      
       //Category menu events start here

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
        private void lbCategorySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //This event occurs when a item is slected in the categories flyout list is selected and navigatio is facilitated as selected

            if (lbItem_ViewFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(DayTours));

            }
            if (lbItem_AddFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(Events));
            }
            if (lbItem_DeleteFavourite.IsSelected)
            {
                MainFrame.Navigate(typeof(InTheCity));
            }
            if (lbItem_Parks.IsSelected)
            {
                MainFrame.Navigate(typeof(ParksAndGarderns));
            }
        }

      //Category menu events end here
 
      //Favourite menu events start here
 
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

        private void tblAddtoFavourites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Favourites));
        }

        private void tblDeleteFavourites_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DeleteFavourites));
        }
     //Favourites menu evetns end here


    }
}
