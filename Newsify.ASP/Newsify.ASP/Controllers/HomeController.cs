using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newsify.ASP.Classes;

namespace Newsify.ASP.Controllers
{
    public class HomeController : Controller
    {
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
                // Log error here
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
                // Log error here
                return RedirectToAction("Index"); // Go back to the Home Page
            }
        }
    }
}
