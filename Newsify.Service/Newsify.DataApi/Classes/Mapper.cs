using System;
using System.Collections.Generic;
using Newsify.DAL;
using Newsify.DataApi.Models;
using Newsify.Logic;
using NLog;

namespace Newsify.DataApi.Classes
{
    public static class Mapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Comment Mapper
        // Map the WebComment to DAL.Comment
        public static Comment MapComment(WebComment wc)
        {
            try
            {
                var c = new Comment()
                {
                    Comment1 = wc.Comment,
                    CommentedAt = wc.CommentedAt,
                    Modified = wc.CommentedAt,
                    Active = true
                };
                return c;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map webcomment to comment failed: " + ex.Message);
                return null; // return nothing to the caller
            }
        }

        // Map a DAL.Comment object to a WebComment object
        public static WebComment MapComment(Comment comment, int articleId, int commentId)
        {
            try
            {
                var da = new DataAccess();
                var author = da.GetAuthor(articleId, commentId);
                var c = new WebComment()
                {
                    Comment = comment.Comment1,
                    Author = author,
                    CommentedAt = comment.CommentedAt,
                    ArticleId = articleId,
                    CommentId = commentId
                };
                return c;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map comment to webcomment failed: " + ex.Message);
                return null; // return nothing to the caller
            }
        }

        // Map a UpdateComment object to a DAL.Comment object
        public static Comment MapComment(UpdateComment uc)
        {
            try
            {
                var c = new Comment()
                {
                    Comment1 = uc.Comment,
                    Modified = uc.Modified,
                    ID = uc.CommentId
                };
                logger.Info("Comment " + uc.CommentId + " updated succesfully.");
                return c;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to update comment " + uc.CommentId + " failed: " + ex.Message);
                return null; // return nothing to the caller
            }
        }
        #endregion Comment Mapper

        #region Article Mapper
        // Map ArticleSource object to DAL.Source object
        public static Source MapSource(ArticleSource source)
        {
            try
            {
                if (source == null)
                    throw new NotImplementedException();

                var src = new Source() { Name = source.Name};
                
                return src;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map ArticleSource " + source.Name + " to Source failed: " + ex.Message);
                return null;
            }
        }

        // Map ArticleCountry object to DAL.Source object
        public static Source MapSource(ArticleCountry country)
        {
            try
            {
                if (country == null)
                    throw new NotImplementedException();

                var src = new Source() { Country = country.Country };

                return src;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map ArticleCountry " + country.Country + " to Source failed: " + ex.Message);
                return null;
            }
        }

        // Map ArticleLanguage object to DAL.Source object
        public static Source MapSource(ArticleLanguage lang)
        {
            try
            {
                if (lang == null)
                    throw new NotImplementedException();

                var src = new Source() { Language = lang.Language };

                return src;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map ArticleLanguage " + lang.Language + " to Source failed: " + ex.Message);
                return null;
            }
        }

        // Map DAL.Article object to WebArticle object
        public static WebArticle MapArticle(DAL.Article article)
        {
            try
            {
                if (article == null)
                    return null;

                WebArticle art = new WebArticle()
                {
                    Title = article.Title,
                    Description = article.Description,
                    Url = article.Url,
                    UrlToImage = article.UrlToImage,
                    ID = article.ID                    
                };
                return art;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map DAL.Article " + article.ID + " to WebArticle failed: " + ex.Message);
                return null;
            }
        }

        // Map List<DAL.Artcle> object to List<WebArticle> object
        public static List<WebArticle> MapArticle(List<Article> articles)
        {
            try
            {
                if (articles == null)
                    return null;

                // Store WebArticles
                var arts = new List<WebArticle>();
                foreach (var a in articles)
                {
                    var art = MapArticle(a);
                    if (art != null)
                        arts.Add(art);
                }
                return arts;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Attempt to map DAL.Article list WebArticle list failed: " + ex.Message);
                return null;
            }
        }
        #endregion Article Mapper
    }
}