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

namespace Newsify.ASP.Models
{

    public class TopTwenty
    {
        public async Task<IEnumerable<Article>> GetTopTwenty()
        {
            string apiKey = "33b909af4e294034ad07bd3546790502"; // My NewsAPI key
            var news = new NewsApiClient(apiKey);
            var top2 = new TopHeadlinesRequest { Country = Countries.US };
            var top = await news.GetTopHeadlinesAsync(top2);
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
}