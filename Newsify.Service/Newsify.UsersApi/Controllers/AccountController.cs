using System;
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
using NLog;

namespace Newsify.UserApi.Controllers
{
    public class AccountController : ApiController
    {
        // for testing ]

        /// <summary>
        /// This is a basic integration test method to ensure connection to the web api.
        /// </summary>
        /// <returns>Returns 'hi' as a string in a 200 response.</returns>
        [HttpGet]
        public IHttpActionResult get()
        {
            return Ok("hi");
        }
        #region Authentication

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This is used by users and admins to log in to the web api for authorization.
        /// </summary>
        /// <param name="user">This is the input model for the user to log in.</param>
        /// <returns>Returns bad request on invalid user model input, or unauthorized for bad user/password combo.
        /// Returns 200 OK on success.</returns>
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
                logger.Info("Invalid password for user " + dbUser.UserName + ", returned Unauthorized.");
                return Unauthorized();
            }
            return BadRequest("User Model isn't valid.");
        }

        /// <summary>
        /// This function is used to log off of an account.
        /// </summary>
        /// <returns>Returns 200 OK on a successful logoff.</returns>
        [HttpGet]
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
                logger.Error(ex, Request.GetOwinContext().Authentication.User.Identity.GetUserName() + " attempted to log off and threw an exception:" + ex.Message);
                return BadRequest("something went wrong.");
            }
        }

        /// <summary>
        /// This allows a new standard user to be registered.
        /// </summary>
        /// <param name="newUser">newUser is of the class Account.</param>
        /// <returns>Returns 200 OK on a successful user creation.</returns>
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
                    Password = "",
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    BirthDate = newUser.BirthDate,
                };
                newsDB.Users.Add(u);
                newsDB.SaveChanges();
            }

            logger.Info(newUser.UserName + " was created.");
            return Ok();
        }

        /// <summary>
        /// This allows a new admin to be registered.  Only an existing Admin can register a new admin.
        /// </summary>
        /// <param name="newAdmin">This is the same as an Account, but it will be registered with Admin permissions.</param>
        /// <returns>Returns 200 OK on success.</returns>
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
                        Password = "",
                        FirstName = newAdmin.FirstName,
                        LastName = newAdmin.LastName,
                        BirthDate = newAdmin.BirthDate,
                    };
                    newsDB.Users.Add(u);
                    newsDB.SaveChanges();
                }

                logger.Info(newAdmin.UserName + " was created as a new admin.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Admin creation threw an exception: " + ex.Message);
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// This function will change a user's password when called.
        /// </summary>
        /// <param name="cp">The ChangePassword model requires the old password, the new password, and a confirmation of the new password to validate.</param>
        /// <returns>Returns 200 OK on a successful password update.</returns>
        [HttpPut]
        [Authorize(Roles = "user")]
        [Route("~/api/Account/Users")]
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
                logger.Error(ex, "A user attempted to change their password and it threw an exception: " + ex.Message);
                return BadRequest("Something went worng while changing the password");
            }
        }

        /// <summary>
        /// This changes password for admins.
        /// </summary>
        /// <param name="cp">Requires old, new, and confirmed new password strings.</param>
        /// <returns>Returns 200 OK on a successful password change.</returns>
        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("~/api/Account/Admins")]
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
                logger.Error(ex, "An admin attempted to change their password and it threw an exception: " + ex.Message);
                return BadRequest("Something went worng while changing the password");
            }
        }

        /// <summary>
        /// If profile information needs to change, this will do that.
        /// </summary>
        /// <param name="profile">First name, last name, and birthdate are updated using this service.</param>
        /// <returns>Returns 200 OK on successful information change.</returns>
        [HttpPut]
        [Authorize(Roles = ("admin, user"))]
        [Route("~/api/Account/Profiles")]
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
                logger.Error(ex, "A user or admin attempted to change their profile and it threw an exception: " + ex.Message);
                return BadRequest("Something went wrong while updating the profile information.");
            }
        }

        /// <summary>
        /// this changes the stored password in the NewsDB database. The password is hashed.
        /// </summary>
        /// <param name="iUser">This passes the IdentityUser into the database.</param>
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
                logger.Error(ex, "An exception was thrown inside of the updatePassword function: " + ex.Message);
                throw ex;
            }
        }
        #endregion
    }
}
