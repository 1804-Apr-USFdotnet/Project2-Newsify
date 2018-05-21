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
    }
}