using System.Linq;
using System.Web.Hosting;
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
        private readonly IStructureInfo _structureInfo;

        const int DefaultPageSize = 2;

        public ActionResult Index(Home model, int? page) {

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            //var container = _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(model.PageLink.Id));
            
            //var news = _repository.Children<IPageModel>(container).OfType<Page>();
            //return View(new HomeViewModel(model, _structureInfo, news.ToPagedList(currentPageIndex, DefaultPageSize), container));
            return View(new DefaultViewModel<Home>(model, _structureInfo));

        }

        public HomeController(IStructureInfo structureInfo) {
            _structureInfo = structureInfo;
        }
    }
}
