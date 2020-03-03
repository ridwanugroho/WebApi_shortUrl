using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using WebAPI.Models;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Url> Url { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UrlStatistic> UrlStatistic { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
