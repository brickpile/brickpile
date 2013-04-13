using System;
using System.Web.Mvc;
using Raven.Contrib.AspNet.Auth;
using Raven.Contrib.AspNet.Auth.Interfaces;

namespace BrickPile.Samples.Controllers
{
    public abstract class Controller : System.Web.Mvc.Controller
    {
        protected readonly IAuthProvider Authenticator;

        protected Controller() {
            Authenticator = new AuthProvider();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            ViewBag.UserName = Authenticator.Current;
            ViewBag.IsAuthenticated = Authenticator.IsAuthenticated;

            base.OnActionExecuting(filterContext);
        }

        protected ActionResult RedirectToLocal(string returnUrl) {
            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            } else {
                return RedirectToAction("Index", "UI");
            }
        }
    }
}
