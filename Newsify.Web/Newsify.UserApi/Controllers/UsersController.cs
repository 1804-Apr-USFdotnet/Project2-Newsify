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

        // GET: api/Users Can't just make an empty get call
        public IHttpActionResult Get()
        {
            return BadRequest("No log in information passed");
        }

        // GET: api/Users/
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

        // POST: api/Users
        public void Post([FromBody] Models.User user)
        {
            try
            {
                if (user != null)
                {
                    var u = db.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password && x.Active).FirstOrDefault();
                    if (u != null)
                    {
                        //return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the Exception error message
            }
            //return null; // User isn't in the database
        }

        // PUT: api/Users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
        }
    }
}
