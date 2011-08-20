using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.UI;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageModel _model;
        private readonly IStructureInfo _structureInfo;

        public ActionResult Index()
        {
            return View(new DefaultViewModel<IPageModel>(_model,_structureInfo));
        }
        public ActionResult Foo() {
            throw new NotImplementedException("This action is not available");
        }
        public HomeController(IPageModel model, IStructureInfo structureInfo) {
            _model = model;
            _structureInfo = structureInfo;
        }
    }
}
