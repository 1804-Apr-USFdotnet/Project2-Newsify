using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebComment
    {
        public int ID { get; set; }
        public DateTime CommentedAt { get; set; }
        public string comment1 { get; set; }
        public DateTime Modified { get; set; }
        public bool Active { get; set; }
    }
}