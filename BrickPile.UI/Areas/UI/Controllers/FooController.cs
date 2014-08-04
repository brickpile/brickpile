using System.Web.Mvc;
using BrickPile.Core.Mvc;

namespace BrickPile.UI.Areas.UI.Controllers
{
    [EditorControls(Disable = true)]
    public class FooController : Controller
    {
        // GET: UI/Foo
        public ActionResult Index()
        {
            return View();
        }
    }
}