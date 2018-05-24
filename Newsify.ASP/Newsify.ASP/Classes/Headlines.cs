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
using Newsify.ASP.Models;
namespace Newsify.ASP.Classes
{

    public class Headlines
    {
        public async Task<List<Models.Article>> GetHeadlinesAsync()
        {

            var request = new TopHeadlinesRequest { Country = Countries.US };
            var articles = await News.GetArticlesAsync(request);

            var top20 = new Articles();

            foreach (var article in articles)
            {
                Models.Article art = new Models.Article();
                art.sourceName = article.Source.Name;
                art.author = article.Author;
                art.title = article.Title;
                art.description = article.Description;
                art.url = article.Url;
                art.urlToImage = article.UrlToImage;
                art.publishedAT = article.PublishedAt.ToString();

                top20.data.Add(art);
            }
            return top20.data;
        }
    }
}