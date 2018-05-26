using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Newsify.ASP.Models
{

    public class Articles
    {

        public List<Article> data { get; set; }
        public Articles() { data = new List<Article>(); }
    }
    public class Article
    {
        public string sourceName { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public string publishedAT { get; set; }
    }

    #region Search Models
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
    #endregion Search Models
}