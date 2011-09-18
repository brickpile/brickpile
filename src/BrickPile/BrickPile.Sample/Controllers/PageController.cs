using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.Infrastructure.Common;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.Sample.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageModel _model;
        private readonly IStructureInfo _structureInfo;
        private readonly IDocumentSession _session;
        public ActionResult Index() {

            return View(new DefaultViewModel<Page>(_model as Page,_structureInfo));
        }
        
        public PageController(IPageModel model, IStructureInfo structureInfo,IDocumentSession session) {
            _model = model;
            _structureInfo = structureInfo;
            _session = session;
        }
    }
}
