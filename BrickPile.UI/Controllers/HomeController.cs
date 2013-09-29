using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Samples.Models;
using Raven.Client;
using Raven.Client.Bundles.Versioning;

namespace BrickPile.Samples.Controllers
{
    public class HomeController : Controller
    {
        private readonly Home _model;
        private readonly IDocumentSession _session;
        //
        // GET: /Home/

        public ActionResult Index() {
            var containers = _session.Query<Container>().ToList();
            ViewBag.Containers = containers;

            var old = _session.Query<Home>();

            //var draft = old.Clone() as Home;
            //draft.Id = "homes/1/draft/";
            
            //_session.Store(draft);
            //_session.SaveChanges();

            //old.Foo = "ändrar en gammal sida..";
            //_session.SaveChanges();

            var provider = GenericHostingEnvironment.GetProvider("DropboxVPP");

            return View(_model);
        }

        public HomeController(Home model, IDocumentSession session) {
            _model = model;
            _session = session;
        }
    }
    public class TheProp {
        public string Test { get; set; }
    }
}
