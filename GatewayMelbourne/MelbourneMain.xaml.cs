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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GatewayMelbourne
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MelbourneMain : Page
    {
        public MelbourneMain()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void imgAusopen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemAusOpen));
        }

        private void Cup_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemAfl));
        }

        private void F1panel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemF1));
        }

        private void MelbourneCuppanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemMelbCup));
        }

        private void LightHousePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemGreatOceanDrive));
        }

        private void Ropewaypanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemMountBuller));
        }

        //private void imgRopeway_Tapped(object sender, TappedRoutedEventArgs e)
        //{

        //}

        private void AquaPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Aquarium));
        }

        private void YarraPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(YarraValley));
        }

        private void PhilipPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ItemPhilipIsland));
        }

        private void LunaParkPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemStkilda));
        }

        private void panelEvents_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Events));
        }

        private void panelIntheCity_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InTheCity));
        }

        private void panelParksAndGardens_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ParksAndGarderns));
        }

        private void panelDayTrips_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DayTours));
        }
    }
}
