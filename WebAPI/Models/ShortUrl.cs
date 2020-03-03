using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ShortUrl
    {
        public string Status { get; set; }
        public string shortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public string Message { get; set; }
    }
}
