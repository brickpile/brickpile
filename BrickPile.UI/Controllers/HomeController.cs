using System.Web.Mvc;
using BrickPile.Samples.Models;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _session;
        //
        // GET: /Home/

        public ActionResult Index(Home currentContent)
        {
            return View(currentContent);
        }
        public HomeController(IDocumentSession session)
        {
            _session = session;
        }
    }
}
