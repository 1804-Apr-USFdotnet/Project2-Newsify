using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using Newsify.DAL;
using Newsify.Logic;
using Newsify.DataApi.Models;
using Newsify.DataApi.Classes;
using NLog;

namespace Newsify.DataApi.Controllers
{
    public class DataController : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        [Route("~api/Data/AddComment")]
        public IHttpActionResult AddComment(WebComment comment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var rc = Mapper.MapComment(comment);
                if (rc != null)
                {
                    var da = new DataAccess();
                    da.AddComment(rc, comment.Author, comment.ArticleId);

                    return Ok();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpGet]
        [Route("~/api/Data/GetComments")]
        public IHttpActionResult GetComments(int articleId)
        {
            try
            {
                DataAccess da = new DataAccess();
                var comments = da.GetComments(articleId);

                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong processing the request.");
            }
        }


    }
}
