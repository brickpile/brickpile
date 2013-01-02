using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using Raven.Client;
using Image = BrickPile.UI.Models.Image;

namespace BrickPile.Sample.Controllers
{
    public class TestController : Controller
    {
        private readonly IDocumentSession _session;

        public ActionResult Index(TestPage currentPage) {

            return View(currentPage);
        }
        
        public TestController(IDocumentSession session) {
            _session = session;
        }
    }

    [PageType(Name = "Test", ControllerType = typeof(TestController))]
    public class TestPage : PageBase {
        
        public Image Image { get; set; }

        public MyImage MyImage { get; set; }

    }
}
