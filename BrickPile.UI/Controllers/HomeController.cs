using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Samples.Models;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _session;
        //
        // GET: /Home/

        public ActionResult Index(Home currentPage) {
            var containers = _session.Query<Container>().ToList();

            ViewBag.Containers = containers;

            return View(currentPage);
        }
        public HomeController(IDocumentSession session) {
            _session = session;
        }
    }
}
