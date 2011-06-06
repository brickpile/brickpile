using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.Controllers {
    [HandleErrorWithElmah]
    public class PageController : Controller {
        private readonly IStructureInfo _structureInfo;
        private readonly IPageModel _current;
        //[OutputCache(Duration = 600,VaryByParam = "none")]
        public ActionResult Index() {
            var model = _current as Page;
            return View(new DefaultViewModel<Page>(model, _structureInfo));
        }
        public PageController(IStructureInfo structureInfo, IPageModel current) {
            _structureInfo = structureInfo;
            _current = current;
        }
    }
}
