using System;
using BrickPile.Core.Identity;
using BrickPile.UI.Areas.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Raven.Client;
using RavenDB.AspNet.Identity;

namespace BrickPile.UI {

    public partial class Startup {

        public static Func<IDocumentSession, UserManager<ApplicationUser>> UserManagerFactory { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/ui/auth/login")
            });

            // configure the user manager
            UserManagerFactory = (session) =>
            {
                var usermanager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(session));
                // allow alphanumeric characters in username               
                usermanager.UserValidator = new UserValidator<ApplicationUser>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

                return usermanager;
            };

        }

        
    }
}