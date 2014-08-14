using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Core.Mvc;
using BrickPile.Samples.Business;
using BrickPile.Samples.Models.ContentTypes;
using BrickPile.Samples.Models.ViewModels;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    [AllowAnonymous, EditorControls]
    public class HomeController : Controller
    {
        private readonly IDocumentStore documentStore;

        public HomeController(IDocumentStore documentStore, INavigationContext navigationContext, IBrickPileContext context)
        {
            this.documentStore = documentStore;
        }

        public ActionResult Index(Page currentPage, Home currentContent)
        {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                HomeViewModel viewModel = session.Load<HomeViewModelTransformer, HomeViewModel>(currentPage.Id);

                return View(viewModel);
            }
        }
    }
}