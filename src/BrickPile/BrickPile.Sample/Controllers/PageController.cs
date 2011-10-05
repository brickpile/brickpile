using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;

namespace BrickPile.Sample.Controllers {
    public class PageController : Controller {
        private readonly Page _model;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View(new PageViewModel(_model, _structureInfo){Class = "page"});
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public PageController(IPageModel model, IStructureInfo structureInfo) {
            _model = model as Page;
            _structureInfo = structureInfo;
        }
    }
}
