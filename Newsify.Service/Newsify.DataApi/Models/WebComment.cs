using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebComment
    {
        [Required]
        public string Comment { get; set; }
        public string Author { get; set; }
        public DateTime CommentedAt { get; set; }
        public int ArticleId { get; set; }
    }
}