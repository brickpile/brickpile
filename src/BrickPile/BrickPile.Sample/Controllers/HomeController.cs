using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class HomeController : BaseController<Home> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {

            var viewModel = new HomeViewModel
                                {
                                    CurrentModel = CurrentModel,
                                    Class = "home",
                                    Hierarchy = Hierarchy
                                };

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public HomeController(IPageModel model, IDocumentSession session)
            : base(model, session) {
        }
    }
}
