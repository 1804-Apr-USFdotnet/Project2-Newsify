using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebArticle
    {
        public int ID { get; set; }
        public Nullable<int> Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishAt { get; set; }
        public string Category { get; set; }
        public string Topic { get; set; }
        public bool Active { get; set; }
    }
}