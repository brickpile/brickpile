using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Controllers {
    public class CaseController : Controller {
        private readonly IStructureInfo _structureInfo;
        private readonly Case _model;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View(new DefaultViewModel<Case>(_model,_structureInfo));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public CaseController(IPageModel model, IStructureInfo structureInfo) {
            _model = model as Case;
            _structureInfo = structureInfo;
        }
    }
}
