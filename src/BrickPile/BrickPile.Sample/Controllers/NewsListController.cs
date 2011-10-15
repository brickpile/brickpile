using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using PagedList;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.Sample.Controllers {
    public class NewsListController : BaseController<NewsList> {
        /// <summary>
        /// Indexes the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public ActionResult Index(int? page) {
            var news = DocumentSession.Query<IPageModel>()
                .Where(model => model.Parent.Id == CurrentModel.Id)
                .OfType<News>();  // query all news pages below the news list

            var pageIndex = page ?? 1; // if no page was specified in the querystring, default to page 1
            var onePageOfNews = news.ToPagedList(pageIndex, 10); // will only contain 10 news per page

            var viewModel = new NewsListViewModel
                                {
                                    CurrentModel = this.CurrentModel,
                                    Hierarchy = this.Hierarchy,
                                    NewsList = onePageOfNews,
                                    Class = "newslist"
                                };

            return View(viewModel);

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsListController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public NewsListController(IPageModel model,IDocumentSession session)
            : base(model, session) {
        }
    }
}
