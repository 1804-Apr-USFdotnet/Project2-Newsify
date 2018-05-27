using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Newsify.ASP.Models
{
    public class WebComment
    {
        [Required]
        public string Comment { get; set; }
        public string Author { get; set; }
        public DateTime CommentedAt { get; set; }
        public int ArticleId { get; set; }
        public int CommentId { get; set; }
    }
}