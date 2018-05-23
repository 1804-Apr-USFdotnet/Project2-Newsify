using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebSource
    {
        public int PK { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
    }
}