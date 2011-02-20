using System.Linq;
using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Example.ViewModels;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.Controllers {

    public class HomeController : Controller {
        private readonly IPageRepository _repository;
        private readonly IStructureInfo _structureInfo;

        const int DefaultPageSize = 2;

        public ActionResult Index(IPageModel model, int? page) {
            if (model == null) {
                model = _repository.Load<dynamic>("pages/1");
            }

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var items = _repository.GetChildren<dynamic>(model).OfType<Page>();

            return View(new HomeViewModel(model,_structureInfo,items.ToPagedList(currentPageIndex, DefaultPageSize)));
        }

        public HomeController(IPageRepository repository, IStructureInfo structureInfo) {
            _repository = repository;
            _structureInfo = structureInfo;
        }
    }
}
