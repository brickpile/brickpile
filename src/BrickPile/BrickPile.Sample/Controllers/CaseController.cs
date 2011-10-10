using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class CaseController : BaseController<Case> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            var viewModel = new DefaultViewModel<Case>
                                {
                                    CurrentModel = CurrentModel,
                                    Hierarchy = Hierarchy
                                };

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public CaseController(IPageModel model, IDocumentSession session)
            : base(model, session) {
        }
    }
}
