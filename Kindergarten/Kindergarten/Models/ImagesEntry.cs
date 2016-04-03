using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kindergarten.Models
{
    public class ImagesEntry
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public int Feedback { get; set; }
        public string Text { get; set; }
    }
}