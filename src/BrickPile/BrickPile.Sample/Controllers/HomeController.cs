using System;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.Infrastructure.Common;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageModel _model;
        private readonly IStructureInfo _structureInfo;
        private readonly IDocumentSession _session;
        private readonly IPageRepository _repository;
        public ActionResult Index()
        {
            return View(new DefaultViewModel<IPageModel>(_model,_structureInfo));
        }
        public ActionResult Foo() {

            var children = _repository.GetChildren(_model);
            children = _session.Query<IPageModel>().Where(x=> x.Parent.Id == _model.Id).Where(model => model.Metadata.PublishedStatus);
            return Json(children,JsonRequestBehavior.AllowGet);

        }
        public HomeController(IPageModel model, IStructureInfo structureInfo, IDocumentSession session, IPageRepository repository) {
            _model = model;
            _structureInfo = structureInfo;
            _session = session;
            _repository = repository;
        }
    }
}
