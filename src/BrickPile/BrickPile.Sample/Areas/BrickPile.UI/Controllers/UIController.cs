using System.Web.Mvc;

namespace BrickPile.UI.Controllers {
    public class UIController : Controller {
        public ActionResult Index() {
            return View("Index");
        }
    }
}
