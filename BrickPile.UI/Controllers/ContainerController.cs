using System.Linq;
using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Samples.Models.ContentParts;
using BrickPile.Samples.Models.ContentTypes;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BrickPile.Samples.Controllers
{
    [AllowAnonymous]
    public class ContainerController : Controller
    {
        private readonly IDocumentStore store;        

        public ContainerController(IDocumentStore store)
        {
            this.store = store;            
        }

        public ActionResult Index(Container currentPage)
        {
            using (IDocumentSession session = this.store.OpenSession())
            {

                //var hero = new Hero
                //{
                //    Heading = "HALCYONDAYS",
                //    Caption = "An exclusive HTML5/CSS3 freebie by Peter Finlan, for Codrops",
                //    ArticleUrl = "http://tempuri.org",
                //    ActionText = "Back to the article"
                //};

                //session.Store(hero);
                //session.SaveChanges();

                var revisions = session.Advanced.Revisions.GetFor<Container>(currentPage.Id).Where(x => x.Metadata.IsPublished);

                var navigationContext = new NavigationContext(ControllerContext.RequestContext);
                var viewModel = new DefaultViewModel<Container>
                {
                    CurrentPage = currentPage,
                    NavigationContext = navigationContext
                };

                return View(viewModel);
            }
        }
    }
}