using EugeneCommunity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace EugeneCommunity.Controllers
{
    
    public class AuthController : Controller
    {
        
        // GET: Auth/LogIn
        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        // POST: Auth/LogIn
        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Don't do this in production!
            if (model.Email == "admin@admin.com" && model.Password == "password")
            {
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, "Brody"),
                new Claim(ClaimTypes.Email, "a@b.com"),
                new Claim(ClaimTypes.Country, "Canada")
            },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }


        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
    }
}