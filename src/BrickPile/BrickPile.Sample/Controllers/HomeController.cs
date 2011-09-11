using System;
using System.Web.Mvc;
using BrickPile.Core.Infrastructure.Common;
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
        public ActionResult Index()
        {
            return View(new DefaultViewModel<IPageModel>(_model,_structureInfo));
        }
        public ActionResult Foo() {
            var children = _session.Query<IPageModel>().GetChildren(_model);
            return Json(children,JsonRequestBehavior.AllowGet);
        }
        public HomeController(IPageModel model, IStructureInfo structureInfo, IDocumentSession session) {
            _model = model;
            _structureInfo = structureInfo;
            _session = session;
        }
    }
}
