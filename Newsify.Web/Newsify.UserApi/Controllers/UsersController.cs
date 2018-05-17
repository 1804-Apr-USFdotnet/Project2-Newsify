using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newsify.DAL;
using Newsify.UserApi.Models;

namespace Newsify.UserApi.Controllers
{
    public class UsersController : ApiController
    {
        private static NewsDBEntities db = new NewsDBEntities();

        // GET: Can't just make an empty get call
        public IHttpActionResult Get()
        {
            return BadRequest("No log in information passed");
        }

        // GET: 
        public HttpResponseMessage Get([FromBody] Models.User user)
        {
            try
            {
                if (user != null)
                {
                    var u = db.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password && x.Active).FirstOrDefault();
                    if (u != null)
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the Exception error message
            }
            return null; // User isn't in the database
        }

        // POST: Process a new user registration
        public IHttpActionResult Post([FromBody] Models.Registration newUser)
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
            }
            return null; // User isn't in the database
        }

        // PUT: The user is only able to update their password, first and last name, and birthday.
        public IHttpActionResult Put([FromBody] Models.Registration profile)
        {
            try
            {
                // Make sure the user is in the database
                var user = db.Users.Where(x => x.UserName == profile.UserName).FirstOrDefault();
                if (user != null)
                {
                    // Update the User's information
                    user.Password = profile.Password;
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    user.BirthDate = profile.Birthdate;

                    db.SaveChanges(); // Saves the changes to the database
                }
                else
                {
                    return NotFound(); // User wasn't found in the database
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
    }
}
