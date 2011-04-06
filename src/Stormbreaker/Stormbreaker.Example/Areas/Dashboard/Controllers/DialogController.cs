using System.Collections.Generic;
using System.Web.Mvc;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Dashboard.Controllers {
    public class DialogController : Controller {
        private readonly IPageRepository _repository;
        private readonly IDropboxRepository _dropboxRepository;

        public ActionResult EditPageReference() {
            return View(new List<IPageModel>(_repository.List<IPageModel>()));
        }

        public ActionResult ListDropboxContent() {
            var metaData = _dropboxRepository.GetMetaData("/dump");

            return View(metaData.Contents);
        }

        public ActionResult UpdateNavigation(FormCollection formCollection) {

            foreach (var a in formCollection) {
                var value = Request.Form[a.ToString()];
            }

            return View();
        }
        public DialogController(IPageRepository pageRepository, IDropboxRepository  dropboxRepository) {
            _repository = pageRepository;
            _dropboxRepository = dropboxRepository;
        }
    }
}