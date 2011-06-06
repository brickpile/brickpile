using System.Web.Mvc;
using Raven.Client;
using Stormbreaker.Models;

namespace Stormbreaker.Dashboard.Controllers {
    public class SettingsController : Controller {
        private readonly IDocumentSession _session;

        public ActionResult Index() {
            ViewBag.Class = "dashboard";
            var model = _session.Load<Settings>("stormbreaker/settings");
            return View(model);
        }

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

        public SettingsController(IDocumentSession session) {
            _session = session;
        }
    }
}
