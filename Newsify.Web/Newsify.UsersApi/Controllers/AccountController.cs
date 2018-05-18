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
        /*private static NewsDBEntities db = new NewsDBEntities();

        #region Http Verbs
        // GET: Can't just make an empty get call
        public IHttpActionResult Get()
        {
            return BadRequest("No log in information passed");
        }

        // GET: 
        public IHttpActionResult Get(Models.User user)
        {
            try
            {
                if (user != null)
                {
                    var u = db.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password && x.Active).FirstOrDefault();
                    if (u != null)
                    {
                        return NotFound(); // Can't login if the user information doesn't match
                    }
                    return Ok(); // Log the user in
                }
            }
            catch (Exception ex)
            {
                // Log the Exception error message
            }
            return BadRequest("Something went wrong."); // User isn't in the database
        }

        // POST: Process a new user registration
        public IHttpActionResult Post([FromBody] Models.Users newUser)
        {
            try
            {
                if (newUser != null)
                {
                    var u = db.Users.Where(x => x.UserName == newUser.UserName).FirstOrDefault();
                    if (u != null)
                    {
                        // Since there is already a user with the provided UserName send a denied message
                        return Conflict();
                    }

                    // Since no user was found, lets move forward with the registration
                    // Create a new DAL.User object that will be pushed to the database
                    DAL.User user = new DAL.User()
                    {
                        UserName = newUser.UserName,
                        Password = newUser.Password,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        BirthDate = newUser.Birthdate,
                        Type = 3,
                        Active = true
                    };
                    db.Users.Add(user); // Add the user to the Users table
                    db.SaveChanges(); // Save the changes to the database
                }
            }
            catch (Exception ex)
            {
                // Log the Exception error message
                return BadRequest("Something went wrong while processing the new user registration request.");
            }
            return Ok(); // Added the user to the database without errors
        }

        // PUT: The user is only able to update their password, first and last name, and birthday.
        public IHttpActionResult Put([FromBody] Models.ChangePassword profile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Make sure the user is in the database
                    var user = db.Users.Where(x => x.UserName == profile.UserName).FirstOrDefault();

                    if (user != null)
                    {
                        // Update the User's password
                        user.Password = profile.NewPassword;

                        db.SaveChanges(); // Saves the changes to the database
                        return Ok(); // Finished processing the request
                    }
                    else
                    {
                        return NotFound(); // User wasn't found in the database
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
            }
            return BadRequest("Something went wrong.");
        }

        // DELETE: To delete a User, change the Active bit to false
        public IHttpActionResult Delete(string userName)
        {
            try
            {
                // Find the User and mark it deleted
                var user = db.Users.Where(x => x.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    user.Active = false; // mark as deleted

                    db.SaveChanges(); // Save changes to the database
                }
                else
                {
                    return NotFound(); // user wasn't found
                }
            }
            catch (Exception ex)
            {
                // Log the exception message here
                return BadRequest("Something went wrong while deleting the user.");
            }
            return Ok(); // Successfully deleted the user
        }
        #endregion

        */
        #region Authentication

        [HttpPost]
        [Route("~/api/Account/Login")]
        public IHttpActionResult Login(DAL.User user)
        {
            if (ModelState.IsValid)
            {
                var userStore = new UserStore<DAL.User>(new UserDBContext());
                var userManager = new UserManager<DAL.User>(userStore);
                var dbUser = userManager.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

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
            var userStore = new UserStore<DAL.User>(new UserDBContext());
            var userManager = new UserManager<DAL.User>(userStore);

            // Before completely registering the user, make sure the username isn't taken
            if (userManager.Users.Any(u => u.UserName == newUser.UserName))
            {
                return BadRequest("Username is taken.");
            }

            userManager.Create(newUser, newUser.Password);

            return Ok();
        }
        #endregion
    }
}
