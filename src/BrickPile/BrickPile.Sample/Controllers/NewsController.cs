using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;

namespace BrickPile.Sample.Controllers {
    public class NewsController : Controller {
        private readonly News _model;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            return View(new NewsViewModel(_model, _structureInfo) { Class = "news" });
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public NewsController(IPageModel model,IStructureInfo structureInfo) {
            _model = model as News;
            _structureInfo = structureInfo;
        }
    }
}
