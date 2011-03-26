using System.Web.Mvc;

namespace Dashboard.Controllers {
    [Authorize]
    public class DashboardController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}
