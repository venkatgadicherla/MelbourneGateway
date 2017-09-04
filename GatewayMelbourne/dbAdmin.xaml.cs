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
    public sealed partial class dbAdmin : Page
    {
        public dbAdmin()
        {
            this.InitializeComponent();
        }

        private void btnDBcreate_Click(object sender, RoutedEventArgs e)
        {
            DbHelperClass db = new DbHelperClass();
            db.CreateDbase();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DbHelperClass db1 = new DbHelperClass();
            db1.deleteData();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            DbHelperClass db1 = new DbHelperClass();
            db1.insertLocations();
        }
    }
}
