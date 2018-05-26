﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newsify.ASP.Classes;
using Newsify.ASP.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Net;
using NLog;
using Newtonsoft.Json;
using NReadability;
using HtmlAgilityPack;

namespace Newsify.ASP.Controllers
{
    public class HomeController : Controller
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
        private static readonly Uri serviceUri = new Uri("http://localhost:3272/");
        private static readonly string cookieName = "ApplicationCookie";
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
                    return View("Index");
                }

                PassCookiesToClient(apiResponse);

                Session["UserName"] = null; // Remove the user from the session memory

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error here
                return RedirectToAction("Index");
            }
        }

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
                    //return View("Error");
                }

                PassCookiesToClient(apiResponse);
                var content = await apiResponse.Content.ReadAsStringAsync();
                Session["UserName"] = content;

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
                    Response.Headers.Add("Set-Cookie", value);
                }
                return true;
            }
            return false;
        }
    }
}
