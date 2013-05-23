using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Samples.Models;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _session;
        private readonly Home _model;
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(_model);
        }
        public HomeController(IDocumentSession session, Home model) {
            _session = session;
            _model = model;
        }
    }
}
