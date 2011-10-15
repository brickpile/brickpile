using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class PageController : BaseController<Page> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            var viewModel = new PageViewModel
                                {
                                    CurrentModel = this.CurrentModel,
                                    Hierarchy = this.Hierarchy,
                                    Class = "page"
                                };

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public PageController(IPageModel model, IDocumentSession session)
            : base(model, session) {
        }
    }
}
