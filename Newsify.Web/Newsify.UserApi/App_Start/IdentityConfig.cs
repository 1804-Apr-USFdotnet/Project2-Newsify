using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newsify.UserApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;

namespace Newsify.UserApi.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new UserDBContext());
            app.CreatePerOwinContext<UserAppManager>(UserAppManager.Create);
            app.CreatePerOwinContext<RoleManager<UserRole>>((options, context) =>
                new RoleManager<UserRole>(
                    new RoleStore<UserRole>(context.Get<UserDBContext>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}