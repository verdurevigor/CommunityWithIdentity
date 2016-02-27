using Antlr.Runtime.Misc;
using EugeneCommunity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace EugeneCommunity
{
    public class Startup
    {
        public static Func<UserManager<Member>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/auth/login")
            });

            // configure the user manager
            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<Member>(
                    new UserStore<Member>(new AppDbContext()));
                // allow alphanumeric characters in username
                usermanager.UserValidator = new UserValidator<Member>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

                return usermanager;
            };
        }
    }
}