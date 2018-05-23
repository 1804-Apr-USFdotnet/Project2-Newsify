using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.DataApi.Models
{
    public class WebUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Active { get; set; }
    }
}