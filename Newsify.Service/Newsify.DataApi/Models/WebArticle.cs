using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public int ID { get; set; }
    }
}