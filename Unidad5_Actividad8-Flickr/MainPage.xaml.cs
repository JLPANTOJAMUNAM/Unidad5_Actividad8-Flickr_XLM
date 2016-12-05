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
using System.Threading.Tasks;
using Unidad5_Actividad8_Flickr.Models;

namespace Unidad5_Actividad8_Flickr
{
    public sealed partial class MainPage : Page
    {
        private HttpClient httpClient;
        private string key = "68235aae25e7a935db219ebce37f5735";

        private string flickrSearchUrl = "https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key={0}&text=<{1}";
        private Task<HttpResponseMessage> txtSearch;
        private object grdFlickrImageOutput;

        //public object PageInvocationService { get; private set; }
        public MainPage()
        {
            this.InitializeComponent();
            btnBuscar.Click += btnBuscar_Click;
        }

        ///Bontón buscar.
        public async void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string url = String.Format(flickrSearchUrl, key, txtBuscar.Text);
            httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                ParseImages(response);
            }
            catch (System.Net.Http.HttpRequestException error)
            {
                //Console.WriteLine(error.Message);
            }
        }

        /// Lista de Imágenes
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
            grdFlickrImagen.ItemsSource = returnedphotos;
        }

        //Pasar a nueva página al dar Click sobre una imágen.
        //Código para el xaml: <GridView x:Name="grdFlickrImagen" Grid.Row="2" Margin="30,0,0,0" SelectionMode="Single" SelectionChanged="grdFlickrImagen_SelectionChanged">
        private void grdFlickrImagen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlickrPhoto selectedPhoto = e.AddedItems.SingleOrDefault() as FlickrPhoto;
            var parameters = new FlickrPhoto();
            parameters.Title = selectedPhoto.Title;
            parameters.ImageUrl = selectedPhoto.ImageUrl;
            Frame.Navigate(typeof(ImgSelec), parameters);
        }
    }
}
