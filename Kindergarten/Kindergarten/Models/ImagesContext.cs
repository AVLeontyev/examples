using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Kindergarten.Models
{
    public class ImagesContext : DbContext
    {
        public ImagesContext()
            : base("Kindergarten")
        {
        }
        public DbSet<ImagesEntry> Entries { get; set; }
    }
}


   