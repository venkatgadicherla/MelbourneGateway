﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
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
    public sealed partial class itemMountBuller : Page
    {
        
        public itemMountBuller()
        {
            this.InitializeComponent();
        }

        private void btnMtBullerDirtns_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnGetDirections_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(GetDirections), tblHeader.Text.ToString());
        }
    }
}
