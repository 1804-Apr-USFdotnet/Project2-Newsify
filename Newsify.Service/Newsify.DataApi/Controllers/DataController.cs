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
        /// <summary>
        /// This will add a comment to the database.
        /// </summary>
        /// <param name="comment">A valid Comment object must be passed.</param>
        /// <returns>Returns 200 OK with a successful</returns>
        [HttpPost]
        [Authorize(Roles = ("admin, user"))]
        [Route("~/api/Data/Add")]
        public IHttpActionResult Comments(WebComment comment)
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

        /// <summary>
        /// This action retrieves all of the comments from an article with the associated article ID.
        /// </summary>
        /// <param name="articleId">The id of the target article.</param>
        /// <returns>Returns 200 OK and comments on a successful query.</returns>
        [HttpGet]
        [Route("~/api/Data/Comments")]
        public IHttpActionResult Comments(int articleId)
        {
            try
            {
                DataAccess da = new DataAccess();
                var coms = da.GetComments(articleId);

                var comments = new List<WebComment>();
                foreach (var c in coms)
                {
                    comments.Add(Mapper.MapComment(c, articleId, c.ID));
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to get comments from article " + articleId + " failed: " + ex.Message);
                return BadRequest("Something went wrong processing the request.");
            }
        }

        /// <summary>
        /// This will grab a specific comment via its id.
        /// </summary>
        /// <param name="gc">GC will get a comment based on both a comment ID and its ArticleID.</param>
        /// <returns>It returns 200 OK and the comment on a successful retrieval.</returns>
        [HttpPost]
        [Route("~/api/Data/Comments")]
        public IHttpActionResult Comments(GetComment gc)
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

        /// <summary>
        /// This is used to update a comment. Users can change a comment's content.
        /// </summary>
        /// <param name="uc">Model UpdateComment will need the comment's id and article id for verification, and the new comment changes.</param>
        /// <returns>Returns 200 OK on a success.</returns>
        [HttpPut]
        [Authorize(Roles = ("admin, user"))]
        [Route("~/api/Data/Comments")]
        public IHttpActionResult Comments(UpdateComment uc)
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

        /// <summary>
        /// This will allow any user to delete their own comment.
        /// </summary>
        /// <param name="commentId">CommentID must match a valid comment's id.</param>
        /// <returns>Returns 200 OK on a success.</returns>
        [HttpDelete]
        [Authorize(Roles = ("admin, user"))]
        [Route("~/api/Data/Comments")]
        public IHttpActionResult Commments(int commentId)
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
        /// <summary>
        /// This will grab articles by source.
        /// </summary>
        /// <param name="source">The source name is passed through this object to match with articles.</param>
        /// <returns>Returns the articles and 200 OK on a success.</returns>
        [HttpPost]
        [Route("~/api/Data/Source")]
        public IHttpActionResult Articles(ArticleSource source)
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

        /// <summary>
        /// Grabs articles from a country.
        /// </summary>
        /// <param name="country">Uses the country abbreviation (e.g. US) to filter articles.</param>
        /// <returns>Returns 200 OK and articles by the listed country.</returns>
        [HttpPost]
        [Route("~/api/Data/Country")]
        public IHttpActionResult Articles(ArticleCountry country)
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

        /// <summary>
        /// Grabs articles by language.
        /// </summary>
        /// <param name="language">Language is a 2 character string, e.g. en for english.</param>
        /// <returns>Returns 200 OK and the requested articles, if any.</returns>
        [HttpPost]
        [Route("~/api/Data/Language")]
        public IHttpActionResult Articles(ArticleLanguage language)
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

        /// <summary>
        /// Grabs and returns article by the requested topic.
        /// </summary>
        /// <param name="topic">Topic is a string to match article topics.</param>
        /// <returns>Returns 200 OK and the requested articles, if there are any to be found.</returns>
        [HttpPost]
        [Route("~/api/Data/Topic")]
        public IHttpActionResult Articles(ArticleTopic topic)
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

        /// <summary>
        /// Grabs articles by title.
        /// </summary>
        /// <param name="title">The string part of the title that the client is searching for.</param>
        /// <returns>Returns 200 OK and the articles that match the title parameter.</returns>
        [HttpPost]
        [Route("~/api/Data/Title")]
        public IHttpActionResult Articles(ArticleTitle title)
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

        /// <summary>
        /// Returns articles based on their publish date.
        /// </summary>
        /// <param name="published">Specifies a date to match with articles.</param>
        /// <returns>Returns 200 OK and matching articles.</returns>
        [HttpPost]
        [Route("~/api/Data/Date")]
        public IHttpActionResult Articles(ArticlePulished published)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Passed information isn't valid.");
                }

                var da = new DataAccess();
                var arts = da.GetArticles(published.PublishedDate);
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
                logger.Error(ex, "Attempt to get articles from published date " + published.PublishedDate + " failed: " + ex.Message);
                return BadRequest("Something went wrong while saving comment.");
            }
        }

        /// <summary>
        /// This allows you to search for an article by its URL, which will return a specific article.
        /// </summary>
        /// <param name="url">Search via a specific URL of an article.</param>
        /// <returns>Returns 200 OK and a specific article.</returns>
        [HttpPost]
        [Route("~/api/Data/ArticleURL")]
        public IHttpActionResult Articles(string url)
        {
            try
            {
                var da = new DataAccess();
                var art = da.GetArticleByUrl(url);
                if (art == null) { return NotFound(); }

                var article = Mapper.MapArticle(art);

                return Ok(article);
            } catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw ex;
            }
        }
        #endregion Articles
    }
}
