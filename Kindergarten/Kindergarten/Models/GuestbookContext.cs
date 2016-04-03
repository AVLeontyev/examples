using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Kindergarten.Models
{
    public class GuestbookContext : DbContext
    {
        public GuestbookContext()
            : base("Kindergarten")
        {
        }
        public DbSet<GuestbookEntry> Entries { get; set; }

    }
}