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
using Windows.ApplicationModel.DataTransfer;
using Unidad5_Actividad8_Flickr.Models;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Unidad5_Actividad8_Flickr
{
    public sealed partial class ImgSelec : Page
    {
        private HttpClient HtpClnt;
        private string Flickerkey = "68235aae25e7a935db219ebce37f5735";
        private string SearchUrl = "http://api.flickr.com/services/rest/?method=flickr.photos.search&api_key={0}&text={1}";

        /// Variables necesarias para la función: "compartir".
        private string strTitleToShare = "Debugmode Flickr Photo Search App";
        private string strDescriptionToShare = "Sharing from Debugmode Flickr Photo Search App";
        private string strsharedText = string.Empty;

        public ImgSelec()
        {
            this.InitializeComponent();
            DataTransferManager manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var urx = (FlickrPhoto)e.Parameter;
            txtTitulo.Text = urx.Title;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += ImgSelec_BackRequested;
        }
        public async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            HtpClnt = new HttpClient();
            HttpResponseMessage GetResponce = await HtpClnt.GetAsync(String.Format(SearchUrl, Flickerkey, txtTitulo.Text));
            ImagePas(GetResponce);
        }

        //Botón: Regresar
        private void ImgSelec_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        //Requerimientos para: Compartir
        void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = strTitleToShare;
            request.Data.Properties.Description = strDescriptionToShare;
            request.Data.SetText(strsharedText);
        }

        //Botón: Compartir
        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            //FlickrPhoto selectedPhoto = e.ClickedItem as FlickrPhoto;
            //var urt = selectedPhoto.ImageUrl;
            //strsharedText = "Hey mira esta foto " + urt + " me encanta #CSharpCornerMvpSummit #Demo";
            DataTransferManager.ShowShareUI();
        }

        //Opción Compartir al dar Click sobre una imágen.
        //En xaml se debe habilitar: <GridView IsItemClickEnabled="True" ItemClick="grdFlickrImageOutput_ItemClick_1">
        private void grdFlickrImageOutput_ItemClick(object sender, ItemClickEventArgs e)
        {
            FlickrPhoto selectedPhoto = e.ClickedItem as FlickrPhoto;
            var urt = selectedPhoto.ImageUrl;
            strsharedText = "Hey mira esta foto " + urt + " me encanta #CSharpCornerMvpSummit #Demo";
            DataTransferManager.ShowShareUI();
        }

        private async void ImagePas(HttpResponseMessage Htpmsg)
        {
            XDocument Doc = XDocument.Parse(await Htpmsg.Content.ReadAsStringAsync());
            var myphoto = from results in Doc.Descendants("photo")
                          select new FlickrSearch
                          {
                              PhotosId = results.Attribute("id").Value.ToString(),
                              ImgTitle = results.Attribute("title").Value.ToString(),
                              Scrt = results.Attribute("secret").Value.ToString(),
                              FarmeId = results.Attribute("farm").Value.ToString(),
                              ServerId = results.Attribute("server").Value.ToString()
                          };
            GridImg.ItemsSource = myphoto;
        }
    }
}
