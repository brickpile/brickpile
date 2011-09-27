using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageModel _model;
        private readonly IStructureInfo _structureInfo;
        private readonly IDocumentSession _session;
        public ActionResult Index() {
            var page = _session.Query<Page>("PageModelWithParentsAndChildren")
                .Include(x => x.Ancestors)
                .Where(x => x.Id == "pages/2")
                .SingleOrDefault();

            return View(new DefaultViewModel<IPageModel>(_model,_structureInfo));
        }
        public HomeController(IPageModel model, IStructureInfo structureInfo, IDocumentSession session) {
            _model = model;
            _structureInfo = structureInfo;
            _session = session;
        }
    }
}
