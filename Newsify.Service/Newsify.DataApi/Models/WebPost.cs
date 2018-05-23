using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebPost
    {
        public int ID { get; set; }
        public int ArticleID { get; set; }
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> ReplyTo { get; set; }
    }
}