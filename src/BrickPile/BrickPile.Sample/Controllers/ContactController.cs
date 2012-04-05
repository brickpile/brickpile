using System.Web.Mvc;
using BrickPile.Sample.Models;
using BrickPile.Sample.ViewModels;
using Raven.Client;

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
                                Hierarchy = this.Hierarchy,
                                Class = "contact"
                            };
            return View(model);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="documentSession">The document session.</param>
        public ContactController(Contact model, IDocumentSession documentSession)
            : base(model, documentSession) { }

    }
}
