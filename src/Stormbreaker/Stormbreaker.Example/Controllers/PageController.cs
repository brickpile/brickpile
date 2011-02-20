using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.Controllers {
    [HandleErrorWithElmah]
    public class PageController : Controller {
        private readonly IStructureInfo _structureInfo;

        [OutputCache(Duration = 600,VaryByParam = "none")]
        public ActionResult Index(Page model) {
            return View(new DefaultViewModel<Page>(model, _structureInfo));
        }

        public PageController(IStructureInfo structureInfo) {
            _structureInfo = structureInfo;
        }

    }
}
