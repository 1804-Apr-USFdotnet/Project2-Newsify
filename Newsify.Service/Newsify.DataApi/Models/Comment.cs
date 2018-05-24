using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    #region Comment Models
    public class WebComment
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public DateTime CommentedAt { get; set; }
        [Required]
        public int ArticleId { get; set; }
        
        public int CommentId { get; set; }
    }

    public class GetComment
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public int ArticleId { get; set; }
    }

    public class UpdateComment
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime Modified { get; set; }
    }
    #endregion Comment Models
}