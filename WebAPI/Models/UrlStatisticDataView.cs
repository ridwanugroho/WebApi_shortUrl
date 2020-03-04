using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class UrlStatisticDataView
    {
        private List<Url> urls;

        public UrlStatisticDataView(List<Url> urls)
        {
            this.urls = urls;
        }

        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public List<Track> Clicked {get; set;}
    }

    public class Track
    {
        public string url { get; set; }
        public string IP { get; set; }
        public int Count { get; set; }
    }
}
