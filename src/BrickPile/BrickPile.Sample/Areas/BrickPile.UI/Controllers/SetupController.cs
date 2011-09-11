/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System.Web.Mvc;
using System.Web.Security;
using BrickPile.UI.Configuration;
using BrickPile.UI.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.UI.Controllers {
    public class SetupController : Controller {
        private readonly IMembershipService _membershipService;
        private readonly IFormsAuthenticationService _formsService;
        private readonly IDocumentSession _session;
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
                    IConfiguration configuration = new Configuration.Configuration();
                    configuration.SiteName = model.Configuration.SiteName;
                    _session.Store(configuration);
                    _session.SaveChanges();

                    return RedirectToAction("index", "Dashboard", new { area = "Dashboard" });
                }

                ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupController"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SetupController(IDocumentSession session) {
            _membershipService = new AccountMembershipService();
            _formsService = new FormsAuthenticationService();
            _session = session;
        }
    }
}
