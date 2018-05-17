using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Newsify.UserApi.Models
{
    // This model will contain the new user's information needed to create a new user
    public class Registration
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
    }
}