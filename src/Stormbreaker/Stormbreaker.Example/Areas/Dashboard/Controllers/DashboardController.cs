using System.Web.Mvc;
using Stormbreaker.Repositories;

namespace Dashboard.Controllers {
    [Authorize]
    public class DashboardController : Controller {
        private readonly IPageRepository _repository;

        public DashboardController(IPageRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index() {
            return View();
        }
    }

}
