using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Newsify.UserApi.Models
{
    // DBContext
    public class UserDBContext : IdentityDbContext<IdentityUser>
    {
        public UserDBContext() : base("UserDB")
        {
        }
    }

    // This model is used to login
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    // This model will be used to create a new user
    public class Account
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastNmae { get; set; }
        public DateTime BirthDate { get; set; }
    }

    // This model will be used to change the user's password
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    // This model will be used to update user's information
    public class UpdateUserProfile
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
    }
}