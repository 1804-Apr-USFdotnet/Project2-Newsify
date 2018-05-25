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
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
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
                    logger.Info("Added comment " + comment.CommentId + " from author " + comment.Author +
                                    " on article " + comment.ArticleId);
                    return Ok();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to add comment by author " + comment.Author + "on article " + comment.ArticleId + " failed: " + ex.Message);
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
                logger.Error(ex, "Attempt to get comments from article " + articleId + " failed: " + ex.Message);
                return BadRequest("Something went wrong processing the request.");
            }
        }

        [HttpPost]
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
                logger.Error(ex, "Attempt to get comment " + gc.CommentId + " failed: " + ex.Message);
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
                {
                    logger.Info("Comment " + uc.CommentId + " was updated.");
                    return Ok();
                }
                return BadRequest("Failed to update the comment.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to update comment " + uc.CommentId + " failed: " + ex.Message);
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
                logger.Info("Comment " + commentId + " successfully deleted.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to delete comment " + commentId + " failed: " + ex.Message);
                return Unauthorized();
            }
        }
        #endregion Comments

        #region Articles
        [HttpPost]
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
                    var arts = da.GetArticles(src);
                    if (arts == null)
                    {
                        return NotFound();
                    }

                    // Let's convert the article objects
                    var articles = Mapper.MapArticle(arts);

                    if (articles == null)
                    {
                        throw new NotImplementedException();
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
                logger.Error(ex, "Attempt to get articles from source " + source.Name + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpPost]
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
                    var arts = da.GetArticles(src);
                    if (arts == null)
                    {
                        return NotFound();
                    }

                    // Let's convert the article objects
                    var articles = Mapper.MapArticle(arts);

                    if (articles == null)
                    {
                        throw new NotImplementedException();
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
                logger.Error(ex, "Attempt to get articles from country " + country.Country + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpPost]
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
                    var arts = da.GetArticles(src);
                    if (arts == null)
                    {
                        return NotFound();
                    }

                    // Let's convert the article objects
                    var articles = Mapper.MapArticle(arts);

                    if (articles == null)
                    {
                        throw new NotImplementedException();
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
                logger.Error(ex, "Attempt to get articles from language " + language.Language + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpPost]
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
                var arts = da.GetArticles(null, topic.Topic);
                if (arts == null)
                {
                    return NotFound();
                }

                // Let's convert the article objects
                var articles = Mapper.MapArticle(arts);

                if (articles == null)
                {
                    throw new NotImplementedException();
                }

                return Ok(articles);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to get articles from topic " + topic.Topic + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpPost]
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
                var arts = da.GetArticles(title.Title);
                if (arts == null)
                {
                    return NotFound();
                }

                // Let's convert the article objects
                var articles = Mapper.MapArticle(arts);

                if (articles == null)
                {
                    throw new NotImplementedException();
                }

                return Ok(articles);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to get articles from title " + title.Title + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        [HttpPost]
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
                var arts = da.GetArticles(pulished.PublishedDate);
                if (arts == null)
                {
                    return NotFound();
                }

                // Let's convert the article objects
                var articles = Mapper.MapArticle(arts);

                if (articles == null)
                {
                    throw new NotImplementedException();
                }

                return Ok(articles);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to get articles from published date " + pulished.PublishedDate + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }
        #endregion Articles
    }
}
