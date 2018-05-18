using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NewsAPI;
using NewsAPI.Models;
using Newsify.DAL;

namespace Newsify.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //test Webhook
            NewsDBEntities newsDB = new NewsDBEntities();
            string apiKey = "33b909af4e294034ad07bd3546790502"; // My NewsAPI key
            //var url3 = "https://newsapi.org/v2/sources?&apiKey=33b909af4e294034ad07bd3546790502";
            //var json3 = new WebClient().DownloadString(url3);
            //JObject jsonResponse = JObject.Parse(json3);
            //var objResponse = jsonResponse["sources"];
            //var art3 = JsonConvert.DeserializeObject<List<Source>>(objResponse.ToString());

            try
            {
                var newsApiClient = new NewsApiClient(apiKey); // init with the api key
                var ss = newsDB.Sources.ToList();
                List<string> sn = new List<string>();
                foreach (var v in ss)
                {
                    sn.Add(v.Id);
                }

                var response = newsApiClient.GetEverything(new EverythingRequest
                {
                    Q = "Amazon",
                    Language = NewsAPI.Constants.Languages.EN,
                    From = new DateTime(2018, 5, 15),
                    PageSize = 100,
                    SortBy = NewsAPI.Constants.SortBys.Relevancy
                });

                var newSources = new List<DAL.Source>();
                foreach (var a in response.Articles)
                {
                    Uri uri = new Uri(a.Url);
                    string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
                    DAL.Source source = new DAL.Source()
                    {
                        Id = a.Source.Id,
                        Name = a.Source.Name,
                        Description = "n/a",
                        Url = requested,
                        Category = "general",
                        Language = "en",
                        Country = "n/a"
                    };

                    bool alreadyAdded = false;
                    foreach (var src in newSources)
                    {
                        if (src.Name == source.Name)
                        { alreadyAdded = true; break; }
                        if (newsDB.Sources.Where(x => x.Name == source.Name).ToList().Count != 0)
                        { alreadyAdded = true; break; }// Already have it in the database
                    }

                    if (!alreadyAdded)
                    {
                        newSources.Add(source);
                    }
                }

                foreach (var src in newSources)
                {
                    newsDB.Sources.Add(src);
                }
                newsDB.SaveChanges();

                List<DAL.Article> articles = new List<DAL.Article>();
                foreach (var article in response.Articles)
                {
                    var source = newsDB.Sources.Where(x => x.Name == article.Source.Name && x.Id == article.Source.Id).FirstOrDefault();
                    int sId;
                    if (source != null && article.Description != null)
                    {
                        sId = source.PK;
                        var art = MapArticle(article, sId, "Technology", "Amazon");
                        articles.Add(art);
                    }                   
                }

                System.Console.WriteLine("Finished grabbing articles about Apple.");
                foreach (var a in articles)
                {
                    newsDB.Articles.Add(a);
                }
                /*foreach (var source in art3)
                {
                    if (source.Description.Length > 200)
                        System.Console.WriteLine(source.Name + " Description is too long.");
                    newsDB.Sources.Add(source);
                }*/
                newsDB.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.WriteLine();
            System.Console.ReadKey();
        }

        public static DAL.Article MapArticle(NewsAPI.Models.Article a, int sId, string cat, string topic)
        {
            DAL.Article art = new DAL.Article()
            {
                Source = sId,
                Author = a.Author,
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                UrlToImage = a.UrlToImage,
                PublishAt = (DateTime)a.PublishedAt,
                Category = cat,
                Topic = topic,
                Active = true
            };

            return art;
        }
    }
}
