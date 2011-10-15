using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class NewsController : BaseController<News> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            var viewModel = new NewsViewModel
                                {
                                    CurrentModel = this.CurrentModel,
                                    Hierarchy = this.Hierarchy,
                                    Class = "news"
                                };

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public NewsController(IPageModel model, IDocumentSession session)
            : base(model ,session) {
        }
    }
}
