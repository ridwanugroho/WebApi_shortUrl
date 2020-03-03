using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using WebAPI.Models;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShorterController : ControllerBase
    {
        private AppDbContext db;
        private object ERR = new
        {
            status = "400",
            message = "bad request"
        };

        public ShorterController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("okok generator");
        }

        [HttpPost("GenerateRandom")]
        public IActionResult GenerateRandom(Url model)
        {
            Console.WriteLine("mencoba . . .");
            Console.WriteLine(model.OriginalUrl);

            int ln = new Random().Next(4, 7);
            var randomUrl = "https://192.168.17.108:5001/" + generateRandom(ln, model.OriginalUrl);

            return Ok(new { randomUrl = randomUrl });
        }

        [HttpPost("GenerateCustom")]
        public IActionResult GenerateCustom(Url url)
        {
            if(!validateOriginalUrl(url.OriginalUrl))
            {
                var _url = (from u in db.Url where u.OriginalUrl == url.OriginalUrl select u).First();

                var ret = new ShortUrl
                {
                    Status = "400",
                    shortUrl = _url.shortUrl,
                    OriginalUrl = url.OriginalUrl,
                    Message = "Original url telah ada"
                };

                return Ok(ret);
            }

            if (!validateShortUrl(url.shortUrl))
            {
                var _url = (from u in db.Url where u.shortUrl == url.shortUrl select u).First();

                var ret = new ShortUrl
                {
                    Status = "400",
                    shortUrl = "",
                    OriginalUrl = url.OriginalUrl,
                    Message = "Short url telah digunakan"
                };

                return Ok(ret);
            }

            else
            {
                var _url = new Url
                {
                    Title = url.OriginalUrl,
                    shortUrl = url.shortUrl,
                    OriginalUrl = url.OriginalUrl,
                    CreatedAt = DateTime.Now,
                };

                db.Url.Add(_url);
                db.SaveChanges();

                _url.shortUrl = "https://localhost:5001/" + _url.shortUrl;

                return Ok(_url);
            }

        }

        private string generateRandom(int ln, string url)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" + url;
            StringBuilder result = new StringBuilder(ln);
            for (int i = 0; i < ln; i++)
                result.Append(characters[random.Next(characters.Length)]);

            return result.ToString();
        }

        private bool validateShortUrl(string url)
        {
            var urls = from u in db.Url select u.shortUrl;

            if (urls.Contains(url))
                return false;

            return true;
        }

        private bool validateOriginalUrl(string url)
        {
            var urls = from u in db.Url select u.OriginalUrl;

            if (urls.Contains(url))
                return false;

            return true;
        }
    }
}