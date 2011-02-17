using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc;
using Stormbreaker.Web.Mvc.ViewModels;

namespace Stormbreaker.Example.Controllers
{
    
    public class HomeController : Controller {
        private readonly IPageRepository _repository;

        public HomeController(IPageRepository repository) {
            _repository = repository;
        }

        public ActionResult Index(Home model) {
            if (model.Id == null)
            {
                model = _repository.Load<Home>("pages/1");
            }
            return View(new DefaultViewModel<Home>(model, _repository));
        }
    }
}
