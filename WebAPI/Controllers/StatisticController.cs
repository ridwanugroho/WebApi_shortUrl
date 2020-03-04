using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private AppDbContext db;

        public StatisticController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var urls = from u in db.Url select u;

            return Ok(urls);
        }

        [HttpGet("{url}")]
        public IActionResult Url(string url)
        {
            var _url = (from u in db.Url where u.shortUrl == url select u).First();

            var urls = from u in db.UrlStatistic where u.ShortUrlId == _url.id.ToString() select u;

            var record = new UrlStatisticDataView(urls.ToList());

            var recordToSend = new
            {
                Click = record.Clicked,
                byDate = record.ByDate,
                byMonth = record.ByMonth,
                byYear = record.ByMonth
            };

            return Ok(recordToSend);
        }

        [HttpGet("track/{url}")]
        public IActionResult Track(string url)
        {
            return Ok(url);
        }
    }
}