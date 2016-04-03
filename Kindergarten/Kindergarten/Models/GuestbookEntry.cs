using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kindergarten.Models
{
    public class GuestbookEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime DateAdded { get; set; }
        public string Reply { get; set; }
        public int Access { get; set; }
    }
}