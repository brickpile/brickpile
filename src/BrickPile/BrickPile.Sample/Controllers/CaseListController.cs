using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using PagedList;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class CaseListController : BaseController<CaseList> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? page) {
            var cases = DocumentSession.Query<IPageModel>()
                .Where(model => model.Parent.Id == CurrentModel.Id)
                .OfType<Case>();  // query all case pages below the case list

            var pageIndex = page ?? 1; // if no page was specified in the querystring, default to page 1
            var onePageOfCases = cases.ToPagedList(pageIndex, 10); // will only contain 10 cases per page

            var viewModel = new CaseListViewModel
                                {
                                    CurrentModel = this.CurrentModel,
                                    Hierarchy = this.Hierarchy,
                                    CaseList = onePageOfCases,
                                    Class = "caselist"
                                };

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseListController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public CaseListController(IPageModel model,IDocumentSession session)
            : base(model, session) {
        }
    }
}
