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
        // Map the WebComment to DAL.Comment
        public static Comment MapComment(WebComment wc)
        {
            try
            {
                var c = new Comment()
                {
                    Comment1 = wc.Comment,
                    CommentedAt = wc.CommentedAt,
                    Modified = wc.CommentedAt
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
    }
}