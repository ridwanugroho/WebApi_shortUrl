using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class UrlStatistic
    {
        public Guid id { get; set; }
        public string ShortUrlId { get; set; }
        public string IpAddress { get; set; }
        public string RefererUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
