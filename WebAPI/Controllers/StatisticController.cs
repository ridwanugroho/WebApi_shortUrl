﻿using System;
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
            var urls = from u in db.UrlStatistic select u;

            return Ok(urls);
        }

        [HttpGet("{url}")]
        public IActionResult Url(string url)
        {
            return Ok(url);
        }

        [HttpGet("track/{url}")]
        public IActionResult Track(string url)
        {
            return Ok(url);
        }
    }
}