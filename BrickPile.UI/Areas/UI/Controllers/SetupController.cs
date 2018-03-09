using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Configuration;
using BrickPile.Core.Identity;
using BrickPile.Core.Mvc;
using BrickPile.UI.Areas.UI.Models;
using BrickPile.UI.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Raven.Client;
using Raven.Client.Documents;

namespace BrickPile.UI.Areas.UI.Controllers {
    [EditorControls(Disable = true)]
    [AllowAnonymous]
    public class SetupController : Controller {

        private readonly IDocumentStore _store;

        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(SetupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (var session = _store.OpenAsyncSession()) {

                using (var userManager = Startup.UserManagerFactory.Invoke(session)) {

                    var user = new ApplicationUser
                    {
                        UserName = model.SetupModel.UserName,
                        Email = model.SetupModel.Email
                    };

                    await session.StoreAsync(user);

                    var result = await userManager.CreateAsync(user, model.SetupModel.Password);

                    if (result.Succeeded) {
                        var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                        GetAuthenticationManager().SignIn(identity);
                        Configuration configuration = new Configuration
                        {
                            SiteName = model.Configuration.SiteName
                        };
                        await session.StoreAsync(configuration);
                        await session.SaveChangesAsync();
                        return RedirectToAction("Index", "UI", new { area = "UI" });
                    }

                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error);
                    }

                    return View();
                }
            }
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            return HttpContext.GetOwinContext().Authentication;
        }

        public SetupController(IDocumentStore store) {
            _store = store;
        }
    }
}
