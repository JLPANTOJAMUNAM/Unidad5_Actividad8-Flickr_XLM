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
using System.Net.Http;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Unidad_Actividad7_Flickr_2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        private string strTitleToShare = "Debugmode Flickr Photo Search App";
        private string strDescriptionToShare = "Sharing from Debugmode Flickr Photo Search App";
        private string strsharedText = string.Empty;
        private HttpClient httpClient;
        private string key = "0c4522b87604077fb92c17f638de688b";

        private string flickrSearchUrl = "https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key={0}&text=<{1}";
        
        public MainPage()
        {
            this.InitializeComponent();
            DataTransferManager manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += manager_DataRequested;
            btnBuscar.Click += btnBuscar_Click;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        void manager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = strTitleToShare;
            request.Data.Properties.Description = strDescriptionToShare;
            request.Data.SetText(strsharedText);
        }
        async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(String.Format(flickrSearchUrl, key, txtSearch.Text));
            ParseImages(response);
        }
        private async void ParseImages(HttpResponseMessage response)
        {
            XDocument xml = XDocument.Parse(await response.Content.ReadAsStringAsync());
            var returnedphotos = from results in xml.Descendants("photo")
                select new FlickrPhoto
                {
                    ImageId = results.Attribute("id").Value.ToString(),
                    FarmId = results.Attribute("farm").Value.ToString(),
                    ServerId = results.Attribute("server").Value.ToString(),
                    Secret = results.Attribute("secret").Value.ToString(),
                    Title = results.Attribute("title").Value.ToString()
                };

            grdFlickrImageOutput.ItemsSource = returnedphotos;
        }

        private void grdFlickrImageOutput_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            FlickrPhoto selectedPhoto = e.ClickedItem as FlickrPhoto;
            var urt = selectedPhoto.ImageUrl;
            //strsharedText = "Hey mira esta foto " + urt + " me encanta #CSharpCornerMvpSummit #Demo";
            //DataTransferManager.ShowShareUI();
            Frame.Navigate(typeof(ImgSelec));
        }
    }
    public class FlickrPhoto
    {
       public string ImageId { get; set; }
       public string FarmId { get; set; }
       public string ServerId { get; set; }
       public string Secret { get; set; }
       public string Title { get; set; }
       public Uri ImageUrl
       {
        get
           {
           return new Uri(string.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}.jpg", FarmId, ServerId, ImageId, Secret));
           }
        }
    }
}
