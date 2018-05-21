using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using Newsify.DAL;
using Newsify.DataApi.Models;
using NLog;

namespace Newsify.DataApi.Controllers
{
    public class DataController : ApiController
    {
        [HttpPost]
        [Route("~api/Data/AddComment")]
        public IHttpActionResult AddComment(WebComment comment)
        {
            try
            {
                using (var uow = new UnitOfWork(new NewsDBEntities()))
                {
                    uow.CommentR.Add(comment);
                    uow.Complete();
                }
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong while saving comment.");
            }
            return null;
        }
    }
}
