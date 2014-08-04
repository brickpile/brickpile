using System.Web.Mvc;
using BrickPile.Core.Mvc;

namespace BrickPile.UI.Areas.UI.Controllers {
    [Authorize, EditorControls(Disable = true)]
    public class UIController : Controller {
        public ActionResult Index() {
            return RedirectToAction("index", "pages");

            if(Request.IsAjaxRequest()) {
                return PartialView("Index");
            }
            return View("Index");
        }
    }
}
