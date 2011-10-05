using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;
using PagedList;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class CaseListController : Controller {
        private readonly CaseList _model;
        private readonly IDocumentSession _session;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? page) {
            var cases = _session.Query<IPageModel>()
                .Where(model => model.Parent.Id == _model.Id)
                .OfType<Case>();  // query all case pages below the case list

            var pageIndex = page ?? 1; // if no page was specified in the querystring, default to page 1
            var onePageOfCases = cases.ToPagedList(pageIndex, 10); // will only contain 10 cases per page

            return View(new CaseListViewModel(_model, _structureInfo, onePageOfCases) { Class = "caselist" });
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseListController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        /// <param name="structureInfo">The structure info.</param>
        public CaseListController(IPageModel model,IDocumentSession session,IStructureInfo structureInfo) {
            _model = model as CaseList;
            _session = session;
            _structureInfo = structureInfo;
        }
    }
}
