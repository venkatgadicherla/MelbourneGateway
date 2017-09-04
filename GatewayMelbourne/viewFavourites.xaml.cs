using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

        private void Page_Loading(FrameworkElement sender, object args)
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

                    tblImageDescription.Margin = new Thickness(5, 5, 5, 5);
                    tblImageDescription.Text = dblocation.locationName;
                    tblImageCategory.Text = " in " + dblocation.category;
                    tblImageCategory.Margin = new Thickness(5);
                    int locationId = dblocation.locationId;


                    pnlInnerLocation.Children.Add(imgLocation);
                    pnlInnerLocation.Children.Add(tblImageDescription);
                    pnlInnerLocation.Children.Add(tblImageCategory);
                    panelFavourite.Children.Add(pnlInnerLocation);

                }
              
            }
        }
    }
}
