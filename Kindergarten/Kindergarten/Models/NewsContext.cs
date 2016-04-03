using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Kindergarten.Models
{
    public class NewsContext : DbContext
    {
        public NewsContext()
            : base("Kindergarten")
        {
        }
        public DbSet<NewsEntry> Entries { get; set; }
    }
}


