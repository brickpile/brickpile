using System.Web.Mvc;

namespace BrickPile.UI.Controllers {
    [Authorize]
    public class UIController : Controller {
        public ActionResult Index() {
            return View("Index");
        }
    }
}
