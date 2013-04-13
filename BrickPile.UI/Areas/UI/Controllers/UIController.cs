using System.Web.Mvc;
using BrickPile.UI.Areas.UI.Models;

namespace BrickPile.UI.Areas.UI.Controllers {
    //[Authorize]
    public class UIController : Controller {

        public ActionResult Index(string returnUrl) {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        //public ActionResult Index() {
        //    //return RedirectToAction("index", "pages");
        //    return View("Index");
        //}
    }
}
