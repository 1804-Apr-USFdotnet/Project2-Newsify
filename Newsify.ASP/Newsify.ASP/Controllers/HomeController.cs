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

        // GET: TopTwenty/Details/5
        public async Task<ActionResult> Index()
        {
            if (top == null)
                top = new Headlines();

            var top20 = await top.GetHeadlinesAsync();

            return View(top20);
        }
        // GET: TopTwenty/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TopTwenty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TopTwenty/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TopTwenty/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TopTwenty/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TopTwenty/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TopTwenty/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
