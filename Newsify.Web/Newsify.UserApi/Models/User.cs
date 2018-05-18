using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Newsify.UserApi.Models
{
    public class UserDBContext : IdentityDbContext<User>
    {
        public UserDBContext() : base("NewsDBEntities")
        {
        }

    }

    public class User : IdentityUser
    {
        public override string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserRole : IdentityRole
    {
        public UserRole() : base() { }
        public UserRole(string name) : base(name) { } 
    }

    public class UserAppManager : UserManager<User>
    {
        public UserAppManager(IUserStore<User> store)
        : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static UserAppManager Create(
            IdentityFactoryOptions<UserAppManager> options, IOwinContext context)
        {
            var manager = new UserAppManager(
                new UserStore<User>(context.Get<UserDBContext>()));

            // optionally configure your manager
            // ...

            return manager;
        }
    }
}