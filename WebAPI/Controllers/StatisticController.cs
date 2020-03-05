using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
        public new IActionResult Url(string url)
        {
            var _url = (from u in db.Url where u.shortUrl == url select u).First();

            var urls = from u in db.UrlStatistic where u.ShortUrlId == _url.id.ToString() select u;

            var record = new UrlStatisticDataView(urls.ToList());

            var recordToSend = new
            {
                Click = record.Clicked,
                byDate = record.ByDate,
                byMonth = record.ByMonth,
                byYear = record.ByYear
            };

            return Ok(recordToSend);
        }

        [HttpGet("track/{url}")]
        public IActionResult Track(string url)
        {
            return Ok(url);
        }

        [Authorize]
        [HttpGet("user")]
        public IActionResult GetUser()
        {
            var token = Request.Headers["Authorization"];
            token = token.ToString().Substring(7);

            var user = GetUserByToken(token);

            var urls = from u in db.Url where u.Owner == user select u;

            return Ok(urls);
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