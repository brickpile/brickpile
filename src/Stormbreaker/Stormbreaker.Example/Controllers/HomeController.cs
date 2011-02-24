using System.Linq;
using System.Web.Mvc;
using Stormbreaker.Configuration;
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
        private readonly IConfiguration _configuration;

        const int DefaultPageSize = 2;

        public ActionResult Index(Home model, int? page) {

            if (model == null) {
                model = _repository.Load<dynamic>(_configuration.HomePageId);
            }

            var currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var container = _repository.Load<IPageModel>(model.PageLink.Id);
            var news = _repository.GetChildren<IPageModel>(container).OfType<Page>();

            return View(new HomeViewModel(model, _structureInfo, news.ToPagedList(currentPageIndex, DefaultPageSize), container));

        }

        public HomeController(IPageRepository repository, IStructureInfo structureInfo, IConfiguration configuration) {
            _repository = repository;
            _structureInfo = structureInfo;
            _configuration = configuration;
        }
    }
}
