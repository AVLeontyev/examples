using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kindergarten.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateAdded { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Image6 { get; set; }
        public string Image7 { get; set; }
        public string Image8 { get; set; }
        public string Image9 { get; set; }
        public string Image10 { get; set; }
        public HttpPostedFileBase upload1 { get; set; }
        public HttpPostedFileBase upload2 { get; set; }
        public HttpPostedFileBase upload3 { get; set; }
        public HttpPostedFileBase upload4 { get; set; }
        public HttpPostedFileBase upload5 { get; set; }
        public HttpPostedFileBase upload6 { get; set; }
        public HttpPostedFileBase upload7 { get; set; }
        public HttpPostedFileBase upload8 { get; set; }
        public HttpPostedFileBase upload9 { get; set; }
        public HttpPostedFileBase upload10 { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}