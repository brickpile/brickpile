using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc.ViewModels;

namespace Stormbreaker.Example.Controllers {
    [HandleErrorWithElmah]
    public class PageController : Controller {

        private readonly IPageRepository _repository;

        public PageController(IPageRepository repository)
        {
            _repository = repository;
        }
        //[OutputCache(Duration = 600,VaryByParam = "none")]
        public ActionResult Index(Page model)
        {
            return View(new DefaultViewModel<Page>(model, _repository));
        }
    }
}
