using System.Web.Mvc;
using Stormbreaker.Dashboard.Models;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Stormbreaker.Dashboard.Controllers {
    public class DialogController : Controller {
        private readonly IPageRepository _repository;
        //private readonly IDropboxRepository _dropboxRepository;

        public ActionResult EditPageReference() {
            var m = new EditPageReferenceModel
                            {
                                CurrentModel = null,
                                BackAction = "Index",
                                Message = "Foo",
                                PageModels = _repository.List<IPageModel>()
                                
                            };
            return View(m);
        }

        //public ActionResult ListDropboxContent() {
        //    var metaData = _dropboxRepository.GetMetaData("/dump");

        //    return View(metaData.Contents);
        //}

        //public DialogController(IPageRepository pageRepository, IDropboxRepository  dropboxRepository) {
        //    _repository = pageRepository;
        //    _dropboxRepository = dropboxRepository;
        //}
    }
}