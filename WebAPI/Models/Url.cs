using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Url
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public string OriginalUrl { get; set; }
        public string shortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Owner { get; set; }
    }
}
