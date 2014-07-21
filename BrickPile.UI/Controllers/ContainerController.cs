using System.Web.Mvc;
using BrickPile.Core.Extensions;
using BrickPile.Samples.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.Samples.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IDocumentStore _store;
        //
        // GET: /Container/

        public ActionResult Index(Container currentPage) {
            using (var session = _store.OpenSession())
            {
                var viewModel = new DefaultViewModel<Container>
                {
                    CurrentPage = currentPage,
                    NavigationContext = session.Advanced.GetNavigationContextFor(currentPage, true)
                };

                return View(viewModel);
            }
        }

        public ContainerController(IDocumentStore store) {
            _store = store;
        }
    }
}
