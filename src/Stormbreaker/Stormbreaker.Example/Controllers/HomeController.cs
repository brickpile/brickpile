using System.Linq;
using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Example.ViewModels;
using Stormbreaker.Extensions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.Controllers {

    public class HomeController : Controller {
        private readonly IPageRepository _repository;
        private readonly IStructureInfo _structureInfo;

        const int DefaultPageSize = 2;

        public ActionResult Index(Home model, int? page) {

            if (model == null) {
                model = (Home) _repository.SingleOrDefault<IPageModel>(x => x.Parent == null);
            }
            return View(new DefaultViewModel<Home>(model, _structureInfo));


            //var currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            //var container = _repository.Load<IPageModel>(model.PageLink.Id);
            //var news = _repository.GetChildren<IPageModel>(container).OfType<Page>();

            //return View(new HomeViewModel(model, _structureInfo, news.ToPagedList(currentPageIndex, DefaultPageSize), container));

        }

        public HomeController(IPageRepository repository, IStructureInfo structureInfo) {
            _repository = repository;
            _structureInfo = structureInfo;
        }
    }
}
