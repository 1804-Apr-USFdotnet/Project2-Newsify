using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    #region Article Models
    // Model to search articles based on source
    public class ArticleSource
    {
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }

    // Model to search articles based on Title
    public class ArticleTitle
    {
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }
    }

    // Model to search articles based on Title
    public class ArticleTopic
    {
        [Required]
        [DataType(DataType.Text)]
        public string Topic { get; set; }
    }

    // Model to search articles based on Title
    public class ArticleCountry
    {
        [Required]
        [StringLength(2)]
        public string Country { get; set; }
    }

    // Model to search articles based on Title
    public class ArticleLanguage
    {
        [Required]
        [StringLength(2)]
        public string Language { get; set; }
    }

    // Model to search articles based on Title
    public class ArticlePulished
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
    }
    #endregion Article Models
}