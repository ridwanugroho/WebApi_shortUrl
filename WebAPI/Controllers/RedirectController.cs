using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Models;
using WebAPI.Data;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class RedirecController : ControllerBase
    {

        private AppDbContext db;
        private ILogger<RedirecController> ilogger;

        public RedirecController(AppDbContext db, ILogger<RedirecController> logger)
        {
            this.db = db;
            ilogger = logger;
        }

        [HttpGet("{url}")]
        public IActionResult Get(string url)
        {
            var oriUrl = generateOriUrl(url);

            if (oriUrl != null)
            {
                recordStatistic(oriUrl);
                return Redirect(oriUrl.OriginalUrl);
            }

            else
                return Ok("null");
        }

        private Url generateOriUrl(string url)
        {
            var _urls = from u in db.Url where u.shortUrl == url select u;

            if (_urls.Count() > 0)
            {
                var _url = _urls.First();
                if (!(_url.OriginalUrl.Contains("http://") || _url.OriginalUrl.Contains("https://")))
                    _url.OriginalUrl = "http://" + _url.OriginalUrl;

                return _url; 
            }

            else
                return null;
        }

        private void recordStatistic(Url url)
        {
            var stat = new UrlStatistic
            {
                ShortUrlId = url.id.ToString(),
                IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                RefererUrl = url.OriginalUrl,
                CreatedAt = DateTime.Now
            };

            ilogger.LogInformation
                (
                $"Short : {url.shortUrl}\n"+
                $"Origin : {stat.RefererUrl}\n"+
                $"Accessed from {stat.IpAddress}"
                );

            db.UrlStatistic.Add(stat);
            db.SaveChanges();
        }
    }
}