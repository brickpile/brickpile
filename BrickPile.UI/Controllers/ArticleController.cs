using System.Linq;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Samples.Models;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IDocumentSession _session;

        /// <summary>
        /// Indexes the specified current content.
        /// </summary>
        /// <param name="currentContent">Content of the current.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public ActionResult Index(Article currentContent) {

            var currentPage = _session.Query<IPageModel>().FirstOrDefault();

            var viewModel = new ArticleViewModel
            {
                Content = currentContent,
                CurrentPage = currentPage
            };

            return View(viewModel);
        }

        //public ActionResult Comment(Comment comment) {
        //    if(ModelState.IsValid) {
        //        _session.Store(comment);
        //        _session.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        public ArticleController(IDocumentSession session) {
            _session = session;
        }

    }
}
