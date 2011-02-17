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
        private readonly IDocumentRepository _documentRepository;

        public FormController(IPageRepository repository, IDocumentRepository documentRepository) {
            _repository = repository;
            _documentRepository = documentRepository;
        }

        public ActionResult Index(Form model) {
            return View(new DefaultViewModel<Form>(model, _repository));
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string title, Form model)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    //byte[] allData = new byte[file.ContentLength];
                    //file.InputStream.Read(allData, 0, allData.Length);
                    //commandInvoker.Execute(new UploadUserImageCommand(currentUserId, model.Title, model.Tags, allData)););
                    var image = new Image(new User { Id = "raven/authorization/users/1" }, title, "a_file_name.png");
                    _documentRepository.Store(image);
                    _documentRepository.SaveChanges();
                    return RedirectToAction("Index", "Form");
                }
                else
                {
                    this.ModelState.AddModelError("file", "Please select a file for upload");
                }
            }
            return View("Index", new DefaultViewModel<Form>(model,_repository));            
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
