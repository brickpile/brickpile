using System.Web.Mvc;

namespace Stormbreaker.Dashboard.Controllers {
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class DashboardController : Controller {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            ViewBag.Class = "dashboard";
            return View();
        }
    }
}
