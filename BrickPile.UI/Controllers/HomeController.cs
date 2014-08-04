using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Core.Extensions;
using BrickPile.Core.Mvc;
using BrickPile.Samples.Models;
using BrickPile.Samples.Models.ViewModels;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    [AllowAnonymous, EditorControls(Disable = false)]
    public class HomeController : Controller
    {
        private readonly IDocumentStore documentStore;
        private readonly INavigationContext navigationContext;

        public HomeController(IDocumentStore documentStore, INavigationContext navigationContext) {
            this.documentStore = documentStore;
            this.navigationContext = navigationContext;
        }

        public ActionResult Index(Home currentPage) {
            HomeViewModel viewModel;
            using (IDocumentSession session = this.documentStore.OpenSession())
            {                                
                //var start = session.Advanced.LoadStartPage();
                //var draft = session.Advanced.LoadDraftFor<Home>(currentPage);
                //var children = session.Advanced.LoadChildrenFor(currentPage).FilterForDisplay();
                //var parent = session.Advanced.LoadParentFor<IPage>(currentPage);
                //var ancestors = session.Advanced.LoadAncestorsFor(currentPage, false);
                //ancestors = session.Advanced.LoadAncestorsFor(currentPage, true);

                //var page = new Container();
                //session.Store(page); // Default RavenDB
                //session.Store(page, StoreAction.None); // Default RavenDB
                //session.Store(page, StoreAction.Save); // Save as draft
                //session.Store(page, StoreAction.Publish); // Publish page/draft
                //session.Store(page, StoreAction.UnPublish); // Unpublish page

                //// Maybe this could be an option
                //session.Store(page, parent, StoreAction.Save); // Add page as child and save

                var context = new NavigationContext(ControllerContext.RequestContext);
                var context2 = ControllerContext.RouteData.GetNavigationContext();
                viewModel = new HomeViewModel
                {
                    CurrentPage = currentPage,
                    NavigationContext = navigationContext
                };
            }


            return View(viewModel);
        }
    }
}