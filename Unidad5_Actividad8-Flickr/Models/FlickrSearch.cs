using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad5_Actividad8_Flickr.Models
{
    class FlickrSearch
    {
        public string PhotosId { get; set; }

        public string ImgTitle { get; set; }

        public string Scrt { get; set; }

        public string FarmeId { get; set; }

        public string ServerId { get; set; }
        public Uri ImgUrl

        {
            get

            {
                return new Uri(string.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}.jpg", FarmeId, ServerId, PhotosId, Scrt));
            }
        }
    }
}
