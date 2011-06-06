using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.Controllers {

    public class HomeController : Controller {
        private readonly IStructureInfo _structureInfo;
        private readonly IPageRepository _pageRepository;

        const int DefaultPageSize = 2;

        //[OutputCache(Duration = 120)]
        
        public ActionResult Index(Home model, int? page) {

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            //var container = _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(model.PageLink.Id));
            
            //var news = _repository.Children<IPageModel>(container).OfType<Page>();
            //return View(new HomeViewModel(model, _structureInfo, news.ToPagedList(currentPageIndex, DefaultPageSize), container));)))

            return View(new DefaultViewModel<Home>(model, _structureInfo));

        }

        public HomeController(IStructureInfo structureInfo, IPageRepository pageRepository, IPageModel current) {
            var currentPage = current as Home;
            _structureInfo = structureInfo;
            _pageRepository = pageRepository;
        }
    }
}
