using System.Web.Mvc;
using Stormbreaker.Example.Models;
using Stormbreaker.Web.Mvc.ViewModels;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Example.Controllers
{
    public class FormController : Controller
    {
        private readonly IStructureInfo _structureInfo;

        public FormController(IStructureInfo structureInfo) {
            _structureInfo = structureInfo;
        }

        public ActionResult Index(Form model) {
            return View(new DefaultViewModel<Form>(model, _structureInfo));
        }

        //public ActionResult Contact(dynamic model, ContactForm form) {
        //    if(ModelState.IsValid) {
        //        RedirectToAction("FormSent");
        //    }
        //    return RedirectToAction("index", new { model });
        //}

        //public ActionResult FormSent() {
        //    throw new NotImplementedException("Det går inte att skicka meddelanden än ...");
        //    //return View(new BaseViewModel<Form>(CurrentModel, _repository));
        //}
    }
}
