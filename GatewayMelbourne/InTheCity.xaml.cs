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
    public sealed partial class InTheCity : Page
    {
        public InTheCity()
        {
            this.InitializeComponent();
        }

        private void hlbtnImgLeftBottom_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemMarket));
        }

        private void hlBtnRightBootm_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemStkilda));
        }

        private void hlbtnImgRightUpper_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemFlindersStreet));
        }

        private void hlbtnLeftUpperImg_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(itemCrown));
        }
    }
}
