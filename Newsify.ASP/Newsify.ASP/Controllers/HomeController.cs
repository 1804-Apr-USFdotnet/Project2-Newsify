using System;
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
