using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageModel _model;
        private readonly IStructureInfo _structureInfo;

        public ActionResult Index() {
            return View(new DefaultViewModel<Page>(_model as Page,_structureInfo));
        }
        
        public PageController(IPageModel model, IStructureInfo structureInfo) {
            _model = model;
            _structureInfo = structureInfo;
        }
    }
}
