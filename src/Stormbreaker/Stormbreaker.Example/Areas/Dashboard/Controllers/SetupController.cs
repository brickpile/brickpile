using System.Web.Mvc;
using Raven.Client;
using Stormbreaker.Models;

namespace Stormbreaker.Dashboard.Controllers {
    public class SetupController : Controller {
        private readonly IDocumentSession _session;

        public ActionResult Index() {
            var settings = _session.Load<Settings>("stormbreaker/settings");
            if(settings == null) {
                settings = new Settings();
            }
            return View(settings);
        }
        public ActionResult Save(Settings settings) {
            if(ModelState.IsValid) {
                _session.Store(settings);
                _session.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index", settings);
        }
        public SetupController(IDocumentSession session) {
            _session = session;
        }
    }
}
