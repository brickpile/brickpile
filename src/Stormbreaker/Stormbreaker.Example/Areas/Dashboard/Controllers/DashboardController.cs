using System.Web.Mvc;

namespace Stormbreaker.Dashboard.Controllers {
    /// <summary>
    /// Default controller for the Dashboard
    /// </summary>
    [Authorize]
    public class DashboardController : Controller {
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            ViewBag.Class = "dashboard";
            return View();
        }
    }
}
