using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Newsify.ASP.Models
{
    public class Search
    {
        [Required]
        public string SearchString { get; set; }
        [Required]
        public string Criteria { get; set; }

    }
}