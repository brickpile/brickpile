using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;
using PagedList;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.Sample.Controllers {
    public class NewsListController : Controller {
        private readonly NewsList _model;
        private readonly IDocumentSession _session;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public ActionResult Index(int? page) {
            var news = _session.Query<IPageModel>()
                .Where(model => model.Parent.Id == _model.Id)
                .OfType<News>();  // query all case pages below the news list

            var pageIndex = page ?? 1; // if no page was specified in the querystring, default to page 1
            var onePageOfNews = news.ToPagedList(pageIndex, 10); // will only contain 10 news per page

            return View(new NewsListViewModel(_model, _structureInfo, onePageOfNews) { Class = "newslist" });
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsListController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        /// <param name="structureInfo">The structure info.</param>
        public NewsListController(IPageModel model,IDocumentSession session, IStructureInfo structureInfo) {
            _model = model as NewsList;
            _session = session;
            _structureInfo = structureInfo;
        }
    }
}
