using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BrickPile.UI.Areas.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Raven.Client;

namespace BrickPile.UI.Areas.UI.Controllers {

    public class AuthController : Controller {

        private readonly IDocumentStore _store;

        // GET: UI/Auth
        [AllowAnonymous, HttpGet]
        public ActionResult LogIn(string returnUrl) {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [AllowAnonymous, HttpPost]
        public async Task<ActionResult> LogIn(LogInModel model) {

            if (!ModelState.IsValid) {
                return View();
            }

            using (var session = _store.OpenSession()) {

                using (var userManager = Startup.UserManagerFactory.Invoke(session)) {

                    var user = await userManager.FindAsync(model.Email, model.Password);

                    if (user != null) {
                        var identity = await userManager.CreateIdentityAsync(
                            user, DefaultAuthenticationTypes.ApplicationCookie);

                        GetAuthenticationManager().SignIn(identity);

                        return Redirect(GetRedirectUrl(model.ReturnUrl));
                    }

                    // user auth failed
                    ModelState.AddModelError("", "Invalid email or password");
                    return View();
                }
            }
        }

        public ActionResult LogOut() {
            GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("index", "home", new { Area = "" });
        }

        private string GetRedirectUrl(string returnUrl) {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl)) {
                return Url.Action("index", "pages", new { Area = "UI" });
            }

            return returnUrl;
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            return HttpContext.GetOwinContext().Authentication;
        }

        public AuthController(IDocumentStore store) {
            _store = store;
        }
    }
}