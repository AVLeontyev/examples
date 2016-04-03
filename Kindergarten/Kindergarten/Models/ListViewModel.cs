using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kindergarten.Models
{
    public class NewsListViewModel
    {
        public IEnumerable<NewsEntry> Entries { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<ImagesEntry> Images { get; set; }
    }

    public class GuestbookListViewModel
    {
        public IEnumerable<GuestbookEntry> Entries { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

}