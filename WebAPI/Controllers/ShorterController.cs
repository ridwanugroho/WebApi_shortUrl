using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using WebAPI.Models;
using WebAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShorterController : ControllerBase
    {
        private AppDbContext db;

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
            var validateOriUrl = validateOriginalUrl(model.OriginalUrl);
            if (validateOriUrl != "")
            {
                var existUrl = "https://192.168.17.108:5001/" + validateOriUrl;

                return BadRequest(existUrl);
            }

            int ln = new Random().Next(4, 7);
            var randomUrl = generateRandom(ln, model.OriginalUrl);

            var urlToSave = new Url
            {
                Title = model.OriginalUrl,
                OriginalUrl = model.OriginalUrl,
                shortUrl = randomUrl,
                CreatedAt = DateTime.Now
            };

            db.Url.Add(urlToSave);
            db.SaveChanges();

            return Ok(new 
            {
                status = 200,
                shortUrl = "https://192.168.17.108:5001/" + randomUrl 
            });
        }

        [Authorize]
        [HttpPost("GenerateCustom")]
        public IActionResult GenerateCustom(Url url)
        {
            var validateOriUrl = validateOriginalUrl(url.OriginalUrl);
            if (validateOriUrl != "")
                return BadRequest("https://192.168.17.108:5001/" + validateOriUrl);

            var validateSrtUrl = validateShortUrl(url.shortUrl);
            if (validateSrtUrl != "")
                return BadRequest("Short URL already in use!");

            else
            {
                var token = Request.Headers["Authorization"];
                token = token.ToString().Substring(7);

                var _url = new Url
                {
                    Title = url.OriginalUrl,
                    shortUrl = url.shortUrl,
                    OriginalUrl = url.OriginalUrl,
                    CreatedAt = DateTime.Now,
                    Owner = GetUserByToken(token)
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

        private string validateShortUrl(string url)
        {
            var urls = from u in db.Url where u.shortUrl == url select u.shortUrl;

            if (urls.Count() > 0)
                return urls.First();

            return "";
        }

        private string validateOriginalUrl(string url)
        {
            Console.WriteLine("Checking for original url : {0}", url);

            var urls = from u in db.Url where u.OriginalUrl == url select u.shortUrl;

            if (urls.Count() > 0)
                return urls.First();

            return "";
        }

        public User GetUserByToken(string token)
        {
            var jwtSecrTokenHandler = new JwtSecurityTokenHandler();
            var secrToken = jwtSecrTokenHandler.ReadToken(token) as JwtSecurityToken;

            var userId = secrToken?.Claims.First(claim => claim.Type == "sub").Value;

            return db.User.Find(Guid.Parse(userId));
        }
    }
}