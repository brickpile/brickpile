using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;

namespace BrickPile.Sample.Controllers {
    public class HomeController : Controller {
        private readonly Home _model;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View(new HomeViewModel(_model,_structureInfo){Class = "home"});
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public HomeController(IPageModel model, IStructureInfo structureInfo) {
            _model = model as Home;
            _structureInfo = structureInfo;
        }
    }
}
