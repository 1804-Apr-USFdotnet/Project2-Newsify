using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{

    public class Program
    {
        public IEnumerable<Article> GetTopTwenty()
        {
            string apiKey = "33b909af4e294034ad07bd3546790502"; // My NewsAPI key
            var news = new NewsApiClient(apiKey);
            var top = news.GetTopHeadlines(new TopHeadlinesRequest { Country = Countries.US });
            Articles arts = new Articles();
            Article art = new Article();

            foreach (var article in top.Articles)
            {
                art.sourceName = article.Source.Name;
                art.author = article.Author;
                art.title = article.Title;
                art.description = article.Description;
                art.url = article.Url;
                art.urlToImage = article.UrlToImage;
                art.publishedAT = article.PublishedAt.ToString();

                arts.data.Add(art);
            }
            return arts.data;
        }



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

        static void Main(string[] args)
        {
            Program pro = new Program();
            var res = pro.GetTopTwenty();
        }
    }
}
