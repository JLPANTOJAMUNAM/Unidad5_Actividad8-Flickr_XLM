using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad5_Actividad8_Flickr.Models
{
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
            set
            {
            }
        }
    }
}
