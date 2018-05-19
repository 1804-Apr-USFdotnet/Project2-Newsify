using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newsify.DAL;
using Newsify.UserApi.Models;

namespace Newsify.UserApi.Controllers
{
    public class AccountController : ApiController
    {
        #region Authentication

        [HttpPost]
        [Route("~/api/Account/Login")]
        public IHttpActionResult Login(DAL.User user)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<IdentityUser>(new UserDBContext());
                var userManager = new UserManager<IdentityUser>(userStore);
                var dbUser = userManager.Users.FirstOrDefault(u => u.UserName == user.UserName && userManager.CheckPassword(u, user.Password));

                if (dbUser == null)
                {
                    return Unauthorized(); // failed to login
                }

                var authManager = Request.GetOwinContext().Authentication;
                var claimsIdentity = userManager.CreateIdentity(dbUser, "ApplicationCookie");

                authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);
                return Ok();
            }
            return BadRequest("User Model isn't valid.");
        }

        [HttpPost]
        [Route("~/api/Account/Register")]
        public IHttpActionResult Register(DAL.User newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model isn't valid.");
            }

            // Start registering the user
            var userStore = new UserStore<IdentityUser>(new UserDBContext());
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = new IdentityUser(newUser.UserName);

            // Before completely registering the user, make sure the username isn't taken
            if (userManager.Users.Any(u => u.UserName == newUser.UserName))
            {
                return BadRequest("Username is taken.");
            }

            // Add user to UserDB
            userManager.Create(user, newUser.Password);
            // Add user to NewsDB so we can link comments to the user
            using (NewsDBEntities newsDB = new NewsDBEntities())
            {
                newsDB.Users.Add(newUser);
                newsDB.SaveChanges();
            }

                return Ok();
        }
        #endregion
    }
}
