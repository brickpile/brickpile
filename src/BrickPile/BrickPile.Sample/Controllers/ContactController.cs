using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using BrickPile.UI;

namespace BrickPile.Sample.Controllers {
    /// <summary>
    /// 
    /// </summary>
    public class ContactController : BaseController<Contact> {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {
            var model = new ContactViewModel
                            {
                                CurrentModel = this.CurrentModel,
                                Pages = this.PublishedPages,
                                Class = "contact"
                            };
            return View(model);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public ContactController(IPageModel model, IStructureInfo structureInfo) : base(model,structureInfo) { }

    }
}
