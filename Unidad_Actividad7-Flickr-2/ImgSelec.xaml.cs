using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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

namespace Unidad_Actividad7_Flickr_2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImgSelec : Page
    {
        public ImgSelec()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += ImgSelec_BackRequested;
        }

        private void ImgSelec_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void grdFlickrImageOutput_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ImgSelec));
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            //FlickrPhoto selectedPhoto = e.ClickedItem as FlickrPhoto;
           // var urt = selectedPhoto.ImageUrl;
            //strsharedText = "Hey mira esta foto " + urt + " me encanta #CSharpCornerMvpSummit #Demo";
            DataTransferManager.ShowShareUI();
        }
    }
}
