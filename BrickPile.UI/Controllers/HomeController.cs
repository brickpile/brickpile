using System.Web.Mvc;
using BrickPile.Samples.Models;

namespace BrickPile.Samples.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(Home currentContent)
        {
            return View(currentContent);
        }

    }
}
