using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public class HomeController : BaseController<Home> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View(new HomeViewModel(CurrentModel,StructureInfo){Class = "home"});
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="currentModel">The current model.</param>
        /// <param name="documentSession">The document session.</param>
        /// <param name="structureInfo">The structure info.</param>
        public HomeController(IPageModel currentModel, IDocumentSession documentSession, IStructureInfo structureInfo) : base(currentModel, documentSession, structureInfo) { }
    }
}
