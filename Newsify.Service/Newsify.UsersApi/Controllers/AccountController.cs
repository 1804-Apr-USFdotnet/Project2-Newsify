using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
        public IHttpActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<IdentityUser>(new UserDBContext());
                var userManager = new UserManager<IdentityUser>(userStore);
                var dbUser = userManager.Users.FirstOrDefault(u => u.UserName == user.UserName);

                if (dbUser == null)
                {
                    return Unauthorized(); // failed to login
                }

                if (userManager.CheckPassword(dbUser, user.Password))
                {
                    var authManager = Request.GetOwinContext().Authentication;
                    var claimsIdentity = userManager.CreateIdentity(dbUser, "ApplicationCookie");

                    authManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);
                    return Ok(dbUser.UserName);
                }
                return Unauthorized();
            }
            return BadRequest("User Model isn't valid.");
        }

        [HttpPost]
        [Route("~/api/Account/Logoff")]
        public IHttpActionResult Logoff()
        {
            try
            {
                Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");

                return Ok();
            }
            catch (Exception ex)
            {
                // log the error here
                return BadRequest("something went wrong.");
            }
        }

        [HttpPost]
        [Route("~/api/Account/Register")]
        public IHttpActionResult Register(Account newUser)
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
            userManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, "user"));

            // Add user to NewsDB so we can link comments to the user
            using (NewsDBEntities newsDB = new NewsDBEntities())
            {
                var u = new DAL.User()
                {
                    UserName = newUser.UserName,
                    Password = user.PasswordHash,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastNmae,
                    BirthDate = newUser.BirthDate,
                    Active = true
                };
                newsDB.Users.Add(u);
                newsDB.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("~/api/Account/RegisterAdmin")]
        public IHttpActionResult RegisterAdmin(Account newAdmin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                // Start registering the new admin
                var userStore = new UserStore<IdentityUser>(new UserDBContext());
                var userManager = new UserManager<IdentityUser>(userStore);
                var user = new IdentityUser(newAdmin.UserName);

                // Before completely registering the user, make sure the username isn't taken
                if (userManager.Users.Any(u => u.UserName == newAdmin.UserName))
                {
                    return BadRequest("Username is taken.");
                }

                // Add user to UserDB
                userManager.Create(user, newAdmin.Password);

                // Let's make the user an admin now
                userManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, "admin"));
                // Add user to NewsDB so we can link comments to the user
                using (NewsDBEntities newsDB = new NewsDBEntities())
                {
                    var u = new DAL.User()
                    {
                        UserName = newAdmin.UserName,
                        Password = user.PasswordHash,
                        FirstName = newAdmin.FirstName,
                        LastName = newAdmin.LastNmae,
                        BirthDate = newAdmin.BirthDate,
                        Active = true
                    };
                    newsDB.Users.Add(u);
                    newsDB.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error here
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        [Route("~/api/Account/ChangePassword")]
        public IHttpActionResult ChangeUserPassword(Models.ChangePassword cp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var userStore = new UserStore<IdentityUser>(new UserDBContext());
                var userManager = new UserManager<IdentityUser>(userStore);

                // Let's find the user so we can't change the password
                var user = userManager.FindById(Request.GetOwinContext().Authentication.User.Identity.GetUserId());
                userManager.ChangePassword(user.Id, cp.OldPassword, cp.NewPassword);

                updatePassword(user); // update the user's password in NewsDB
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error here
                return BadRequest("Something went worng while changing the password");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("~/api/Account/ChangeAdminPassword")]
        public IHttpActionResult ChangeAdminPassword(Models.ChangePassword cp)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var userStore = new UserStore<IdentityUser>(new UserDBContext());
                var userManager = new UserManager<IdentityUser>(userStore);

                // Let's find the user so we can't change the password
                var user = userManager.FindById(Request.GetOwinContext().Authentication.User.Identity.GetUserId());
                userManager.ChangePassword(user.Id, cp.OldPassword, cp.NewPassword);

                updatePassword(user); // update the user's password in NewsDB

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the error here
                return BadRequest("Something went worng while changing the password");
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        [Route("~/api/Account/UpdateProfile")]
        public IHttpActionResult UpdateProfile(Models.UpdateUserProfile profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't passed.");
                }

                using (NewsDBEntities newsDB = new NewsDBEntities())
                {
                    var user = newsDB.Users.FirstOrDefault(u => u.UserName == Request.GetOwinContext().Authentication.User.Identity.GetUserName());
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    user.BirthDate = profile.BirthDate;

                    // Save the changes to the database
                    newsDB.SaveChanges();
                }
                    return Ok();
            }
            catch (Exception ex)
            {
                // log the error here
                return BadRequest("Something went wrong while updating the profile information.");
            }
        }

        private void updatePassword(IdentityUser iUser)
        {
            try
            {
                // Change the user's password in NewsDB database
                using (NewsDBEntities newsDB = new NewsDBEntities())
                {
                    var user = newsDB.Users.FirstOrDefault(u => u.UserName == iUser.UserName);
                    user.Password = iUser.PasswordHash;
                    newsDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;// log error here
            }
        }
        #endregion
    }
}
