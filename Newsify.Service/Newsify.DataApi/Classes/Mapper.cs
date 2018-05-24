using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Newsify.DAL;
using Newsify.DataApi.Models;

namespace Newsify.DataApi.Classes
{
    public static class Mapper
    {
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
                // log error here
                return null; // return nothing to the caller
            }
        }

        // Map a DAL.Comment object to a WebComment object
        public static WebComment MapComment(Comment comment, int articleId, int commentId)
        {
            try
            {
                var c = new WebComment()
                {
                    Comment = comment.Comment1,
                    CommentedAt = comment.CommentedAt,
                    ArticleId = articleId,
                    CommentId = commentId
                };
                return c;
            }
            catch (Exception ex)
            {
                // log error here
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
                return c;
            }
            catch (Exception ex)
            {
                // log error here
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
                // log error here
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
                // log error here
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
                // log error here
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
                    UrlToImage = article.UrlToImage
                };
                return art;
            }
            catch (Exception ex)
            {
                // Log error here
                return null;
            }
        }
        #endregion Article Mapper
    }
}