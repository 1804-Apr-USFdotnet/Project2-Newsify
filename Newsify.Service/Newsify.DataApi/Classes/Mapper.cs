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
    public class Mapper
    {
        public Comment MapComment(WebComment wc)
        {
            try
            {
                var date = DateTime.Now;
                var c = new Comment()
                {
                    Comment1 = wc.ToString(),
                    CommentedAt = date,
                    Modified = date
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