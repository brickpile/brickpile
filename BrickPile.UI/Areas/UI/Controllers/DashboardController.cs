using System.Web.Mvc;
using BrickPile.Core.Mvc;

namespace BrickPile.UI.Areas.UI.Controllers {
    /// <summary>
    /// Default controller for the Dashboard
    /// </summary>
    [Authorize, EditorControls(Disable = true)]
    public class DashboardController : Controller {
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            if(this.Request.IsAjaxRequest()) {
                return PartialView();
            }
            return View();
        }
    }
}
