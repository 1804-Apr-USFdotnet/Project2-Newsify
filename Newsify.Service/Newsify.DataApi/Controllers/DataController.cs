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
        #region Comments
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        [Route("~/api/Data/AddComment")]
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

        [HttpGet]
        [Route("~/api/Data/GetComment")]
        public IHttpActionResult GetComment(GetComment gc)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                DataAccess da = new DataAccess();
                var comment = da.GetComment(gc.CommentId); // grab the comment

                if (comment == null)
                {
                    return NotFound();
                }

                // Map the comment to WebComment before returning it to the client
                var comm = Mapper.MapComment(comment, gc.ArticleId, gc.CommentId);

                return Ok(comm);
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong processing the request.");
            }
        }

        [HttpPut]
        [Authorize(Roles = ("admin, user"))]
        [Route("~/api/Data/UpdateComment")]
        public IHttpActionResult UpdateComment(UpdateComment uc)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                DataAccess da = new DataAccess();

                if (da.UpdateComment(Mapper.MapComment(uc)))
                    return Ok();
                return BadRequest("Failed to update the comment.");
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong processing the request.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = ("admin, user"))]
        [Route("~/api/Data/DeleteComment")]
        public IHttpActionResult DeleteCommment(int commentId)
        {
            try
            {
                DataAccess da = new DataAccess();
                da.DeleteComment(commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log error here
                return Unauthorized();
            }
        }
        #endregion Comments

        #region Articles
        [HttpGet]
        [Route("~/api/Data/GetArticlesBySourceName")]
        public IHttpActionResult GetArticles(ArticleSource source)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var src = Mapper.MapSource(source);
                if (src != null)
                {
                    var da = new DataAccess();
                    var articles = da.GetArticles(src);
                    if (articles == null)
                    {
                        return NotFound();
                    }

                    return Ok(articles);
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
        [Route("~/api/Data/GetArticlesByCountry")]
        public IHttpActionResult GetArticles(ArticleCountry country)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var src = Mapper.MapSource(country);
                if (src != null)
                {
                    var da = new DataAccess();
                    var articles = da.GetArticles(src);
                    if (articles == null)
                    {
                        return NotFound();
                    }

                    return Ok(articles);
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
        [Route("~/api/Data/GetArticlesByLanguage")]
        public IHttpActionResult GetArticles(ArticleLanguage language)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var src = Mapper.MapSource(language);
                if (src != null)
                {
                    var da = new DataAccess();
                    var articles = da.GetArticles(src);
                    if (articles == null)
                    {
                        return NotFound();
                    }

                    return Ok(articles);
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
        [Route("~/api/Data/GetArticlesByTopic")]
        public IHttpActionResult GetArticles(ArticleTopic topic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var da = new DataAccess();
                var articles = da.GetArticles(null, topic.Topic);
                if (articles == null)
                {
                    return NotFound();
                }

                return Ok(articles);
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpGet]
        [Route("~/api/Data/GetArticlesByTitle")]
        public IHttpActionResult GetArticles(ArticleTitle title)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var da = new DataAccess();
                var articles = da.GetArticles(title.Title);
                if (articles == null)
                {
                    return NotFound();
                }

                return Ok(articles);
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpGet]
        [Route("~/api/Data/GetArticlesByPublishedDate")]
        public IHttpActionResult GetArticles(ArticlePulished pulished)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var da = new DataAccess();
                var articles = da.GetArticles(pulished.PublishedDate);
                if (articles == null)
                {
                    return NotFound();
                }

                return Ok(articles);
            }
            catch (Exception ex)
            {
                // Log error here
                return BadRequest("Something went wrong while saving comment.");
            }
        }
        #endregion Articles
    }
}
