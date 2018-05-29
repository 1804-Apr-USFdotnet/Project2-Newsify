using HtmlAgilityPack;
using Newsify.ASP.Classes;
using Newsify.ASP.Models;
using Newtonsoft.Json;
using NLog;
using NReadability;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Newsify.ASP.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
        private static readonly Uri serviceUri = new Uri("http://ec2-18-205-108-130.compute-1.amazonaws.com/Newsify.UsersApi_deploy/");
        private static readonly string cookieName = ".AspNet.ApplicationCookie";
        private static Logger logger = LogManager.GetCurrentClassLogger();

        Headlines top = new Headlines();

        // GET: Current Top Headlines
        public async Task<ActionResult> Index()
        {
            if (top == null)
                top = new Headlines();

            var top20 = await top.GetHeadlinesAsync();

            return View(top20);
        }

        // GET: Change to the About Us view
        public ActionResult About()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "About view failed: " + ex.Message);
                return RedirectToAction("Index"); // Go back to the Home Page
            }
        }

        // GET: Change to the Contact Us view
        public ActionResult Contact()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Contact view failed: " + ex.Message);
                return RedirectToAction("Index"); // Go back to the Home Page
            }
        }

        // GET: Change to the Login view
        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Login view failed: " + ex.Message);
                return RedirectToAction("Index"); // Go back to the Home Page
            }
        }

        // GET: Change to create new user view
        public ActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Register view failed: " + ex.Message);
                return RedirectToAction("Index"); // Go back to the home page
            }
        }

        // GET: Log the user out
        public async Task<ActionResult> Logout()
        {
            try
            {
                HttpRequestMessage requestMessage = CreateRequestToService(HttpMethod.Post, "api/Account/Logoff");

                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await client.SendAsync(requestMessage);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Attempt to send logoff message failed: " + ex.Message);
                    return View("Index");
                }

                PassCookiesToClient(apiResponse);

                Session["UserName"] = null; // Remove the user from the session memory

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Logout failed: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: Show the contents of the article for reading
        public ActionResult Read(Article article)
        {
            try
            {
                var t = new NReadabilityWebTranscoder();
                bool b;
                string page = t.Transcode(article.url, out b);

                if (b)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);

                    var mainText = doc.DocumentNode.SelectSingleNode("//div[@id='readInner']").InnerText;
                    article.mainText = mainText;
                }

                return View(article);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Error");
            }
        }

        // Get: Change to New Comment View
        public ActionResult Comment(int articleId)
        {
            try
            {             
                var comment = new WebComment() { Author = Session["UserName"].ToString(), ArticleId = articleId};
                return View(comment);
            }
            catch (Exception ex)
            {
                // Log error here
                logger.Error(ex.Message);
                return View("Error");
            }
        }

        // GET: Get all of the comments for the selected article
        public async Task<ActionResult> Comments(int articleID)
        {
            try
            {
                var comments = new List<WebComment>();

                var requestMessage = CreateRequestToService(HttpMethod.Get, "api/Data/Comments?articleID=" + articleID);

                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await client.SendAsync(requestMessage);
                    if (!apiResponse.IsSuccessStatusCode)
                    {
                        throw new Exception("Request failed with following error: " + apiResponse.StatusCode);
                    }

                    var content = await apiResponse.Content.ReadAsStringAsync();
                    comments = JsonConvert.DeserializeObject<List<WebComment>>(content);
                    return PartialView(comments);
                }
                catch (Exception ex)
                {
                    // Log error here
                    logger.Error(ex, ex.Message);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // log error here
                logger.Error(ex, ex.Message);
                return View("Error");
            }
        }

        // Add the Comment to the Database
        [HttpPost]
        public async Task<ActionResult> Comment(WebComment model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Enter information isn't valid.");
                }

                // Let's set the author and CommentedAt properties now
                model.Author = Session["UserName"].ToString();
                model.CommentedAt = DateTime.Now;

                HttpRequestMessage requestMessage = CreateRequestToService(HttpMethod.Post, "api/Data/Add");
                requestMessage.Content = new ObjectContent<WebComment>(model, new JsonMediaTypeFormatter());
                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await client.SendAsync(requestMessage);
                    if (!apiResponse.IsSuccessStatusCode)
                    {
                        throw new Exception("Request failed with following error: " + apiResponse.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    // Log error here
                    logger.Error(ex.Message);
                    return View("Error");
                }

                return RedirectToAction("Index"); // For now just go back to the main page
            }
            catch (Exception ex)
            {
                // Log error here
                logger.Error(ex.Message);
                return View("Error");
            }
        }

        // Search for articles based on the criteria
        public async Task<ActionResult> Search(Search search)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new NotSupportedException(); // Invalid search, throw an error
                }

                // Search for the articles in the database
                HttpRequestMessage requestMessage = CreateSearchRequestToService(search);
                if (requestMessage == null)
                {
                    throw new Exception("Failed to create a Search request to send to the DataAPi.");
                }

                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await client.SendAsync(requestMessage);
                    if (!apiResponse.IsSuccessStatusCode)
                    {
                        throw new Exception("Request failed with following error: " + apiResponse.StatusCode);
                    }

                    // get the articles from the response body
                    var content = await apiResponse.Content.ReadAsStringAsync(); // get the json string from the content

                    var articles = JsonConvert.DeserializeObject<List<Article>>(content);

                    return View(articles);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception thrown by Search: " + ex.Message);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View("Error");
            }
        }

        // Log the user in
        [HttpPost]
        public async Task<ActionResult> Login(LogIn user)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();

                // Let's log in the user
                HttpRequestMessage requestMessage = CreateRequestToService(HttpMethod.Post, "api/Account/Login");
                requestMessage.Content = new ObjectContent<LogIn>(user, new JsonMediaTypeFormatter());

                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await client.SendAsync(requestMessage);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception thrown by Login: " + ex.Message);
                    return View("Index");
                }

                if (!apiResponse.IsSuccessStatusCode)
                {
                    return View("Login");
                }

                PassCookiesToClient(apiResponse);
                var content = await apiResponse.Content.ReadAsStringAsync();
                Session["UserName"] = content.Replace("\"", ""); // Remove \" from UserName

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Some other Login error occurred: " + ex.Message);
                return RedirectToAction("Index"); // Go back to the Home Page
            }
        }

        // Register a new user
        [HttpPost]
        public async Task<ActionResult> Register(NewAccount account)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception();
                HttpRequestMessage requestMessage = CreateRequestToService(HttpMethod.Post, "api/Account/Register");
                requestMessage.Content = new ObjectContent<NewAccount>(account, new JsonMediaTypeFormatter());

                HttpResponseMessage apiResponse;
                try
                {
                    apiResponse = await client.SendAsync(requestMessage);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Register failed on client.SendAsync: " + ex.Message);
                    return View("Index");
                }

                if (!apiResponse.IsSuccessStatusCode)
                {
                    //return View("Error");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Register view failed: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        private HttpRequestMessage CreateRequestToService(HttpMethod method, string uri)
        {
            var apiRequest = new HttpRequestMessage(method, new Uri(serviceUri, uri));

            string cookieValue = Request.Cookies[cookieName]?.Value ?? ""; // ?. operator new in C# 7

            apiRequest.Headers.Add("Cookie", new CookieHeaderValue(cookieName, cookieValue).ToString());

            return apiRequest;
        }

        private HttpRequestMessage CreateSearchRequestToService(Search search)
        {
            try
            {
                HttpRequestMessage requestMessage = CreateRequestToService(HttpMethod.Post, "api/Data/" + search.Criteria);
                if (search.Criteria == "Title")
                {
                    var at = new ArticleTitle() { Title = search.SearchString };
                    requestMessage.Content = new ObjectContent<ArticleTitle>(at, new JsonMediaTypeFormatter());
                }
                else if (search.Criteria == "Topic")
                {
                    var at = new ArticleTopic() { Topic = search.SearchString };
                    requestMessage.Content = new ObjectContent<ArticleTopic>(at, new JsonMediaTypeFormatter());
                }
                else if (search.Criteria == "Source")
                {
                    var at = new ArticleSource() { Name = search.SearchString };
                    requestMessage.Content = new ObjectContent<ArticleSource>(at, new JsonMediaTypeFormatter());
                }
                else if (search.Criteria == "Country")
                {
                    var at = new ArticleCountry() { Country = search.SearchString };
                    requestMessage.Content = new ObjectContent<ArticleCountry>(at, new JsonMediaTypeFormatter());
                }
                else if (search.Criteria == "Language")
                {
                    var at = new ArticleLanguage() { Language = search.SearchString };
                    requestMessage.Content = new ObjectContent<ArticleLanguage>(at, new JsonMediaTypeFormatter());
                }
                else if (search.Criteria == "Date")
                {
                    var at = new ArticlePulished() { PublishedDate = Convert.ToDateTime(search.SearchString) };
                    requestMessage.Content = new ObjectContent<ArticlePulished>(at, new JsonMediaTypeFormatter());
                }

                return requestMessage;
            }
            catch (Exception ex)
            {
                // log the error here
                logger.Error(ex.Message);
                return null;
            }
        }

        private bool PassCookiesToClient(HttpResponseMessage apiResponse)
        {
            if (apiResponse.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                foreach (string value in values)
                {
                    Request.Headers.Add("Set-Cookie", value);
                }
                return true;
            }
            return false;
        }
    }
}
