using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using System.Threading.Tasks;
using Newsify.ASP.Classes;

namespace Newsify.ASP.Models
{

    public class Articles
    {

        public List<Article> data { get; set; }
        public Articles() { data = new List<Article>(); }
    }
    public class Article
        {
            public string sourceName { get; set; }
            public string author { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string urlToImage { get; set; }
            public string publishedAT { get; set; }
        }
}