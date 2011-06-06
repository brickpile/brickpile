using System.Web.Mvc;
using Raven.Client;
using Stormbreaker.Models;

namespace Stormbreaker.Dashboard.Controllers {
    /// <summary>
    /// Common settings for Stormbreaker
    /// </summary>
    public class SettingsController : Controller {
        private readonly IDocumentSession _session;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            ViewBag.Class = "dashboard";
            var model = _session.Load<Settings>("stormbreaker/settings");
            return View(model);
        }
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Save(Settings model) {
            if(!ModelState.IsValid) {
                return View("index", model);
            }

            var settings = _session.Load<Settings>(model.Id);

            UpdateModel(settings);
            _session.Store(settings);
            _session.SaveChanges();

            return RedirectToAction("index");            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SettingsController(IDocumentSession session) {
            _session = session;
        }
    }
}
