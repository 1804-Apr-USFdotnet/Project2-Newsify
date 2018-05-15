using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newsify.DAL;

namespace Newsify.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            NewsDBEntities newsDB = new NewsDBEntities();
            string apiKey = "33b909af4e294034ad07bd3546790502"; // My NewsAPI key
            var url3 = "https://newsapi.org/v2/sources?&apiKey=33b909af4e294034ad07bd3546790502";
            var json3 = new WebClient().DownloadString(url3);
            JObject jsonResponse = JObject.Parse(json3);
            var objResponse = jsonResponse["sources"];
            var art3 = JsonConvert.DeserializeObject<List<Source>>(objResponse.ToString());

            try
            {
                foreach (var source in art3)
                {
                    if (source.Description.Length > 200)
                        System.Console.WriteLine(source.Name + " Description is too long.");
                    newsDB.Sources.Add(source);
                }
                newsDB.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.WriteLine();
            System.Console.ReadKey();
        }
    }
}
