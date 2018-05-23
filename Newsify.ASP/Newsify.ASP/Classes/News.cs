using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System.Threading.Tasks;

namespace Newsify.ASP.Classes
{
    public static class News
    {
        private static string apiKey = ConfigurationManager.AppSettings.Get("apiKey");
        private static NewsApiClient client = new NewsApiClient(apiKey);

        public static async Task<IEnumerable<Article>> GetArticlesAsync(TopHeadlinesRequest request)
        {
            var articles = await client.GetTopHeadlinesAsync(request);
            return articles.Articles.AsEnumerable();
        }

        public static async Task<IEnumerable<Article>> GetArticlesAsync(EverythingRequest request)
        {
            var articles = await client.GetEverythingAsync(request);
            return articles.Articles.AsEnumerable();
        }
    }
}