using System.Web.Mvc;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Controllers {
    /// <summary>
    /// 
    /// </summary>
    public class ContactController : Controller {
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public ActionResult Index(Contact currentPage) {

            var viewModel = new DefaultViewModel<Contact>
            {
                //CurrentPage = currentPage,
                NavigationContext = _structureInfo.NavigationContext
            };

            ViewBag.Class = "contact";

            return View(viewModel);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="structureInfo">The structure info.</param>
        public ContactController(IStructureInfo structureInfo) {
            _structureInfo = structureInfo;
        }
    }
}
