using System.Web.Mvc;
using System.Web.Security;
using BrickPile.Core.Configuration;
using BrickPile.UI.Areas.UI.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.UI.Areas.UI.Controllers {
    public class SetupController : Controller {
        private readonly IMembershipService _membershipService;
        private readonly IFormsAuthenticationService _formsService;
        private readonly IDocumentStore _store;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            // Add check for when a site is configured and you enter the path /dashboard/setup
            // Add an awesome view
            return View();
        }
        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(SetupViewModel model) {
            if (ModelState.IsValid) {
                // Attempt to register the user
                MembershipCreateStatus createStatus = _membershipService.CreateUser(model.SetupModel.UserName, model.SetupModel.Password, model.SetupModel.Email);

                if (createStatus == MembershipCreateStatus.Success) {

                    _formsService.SignIn(model.SetupModel.UserName, false /* createPersistentCookie */);
                    // Create the site configuration
                    using (var session = _store.OpenSession()) { 
                        IConfiguration configuration = new Configuration();
                        configuration.SiteName = model.Configuration.SiteName;
                        session.Store(configuration);
                        session.SaveChanges();
                    }

                    return RedirectToAction("index", "UI", new { area = "ui" });
                }

                ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupController" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public SetupController(IDocumentStore store) {
            _membershipService = new AccountMembershipService();
            _formsService = new FormsAuthenticationService();
            _store = store;
        }
    }
}
