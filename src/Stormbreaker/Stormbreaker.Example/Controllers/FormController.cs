using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.RavenDBMembership;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Mvc.ViewModels;

namespace Stormbreaker.Example.Controllers
{
    public class FormController : Controller
    {
        private readonly IPageRepository _repository;

        public FormController(IPageRepository repository) {
            _repository = repository;
        }

        public ActionResult Index(Form model) {
            return View(new DefaultViewModel<Form>(model, _repository));
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
