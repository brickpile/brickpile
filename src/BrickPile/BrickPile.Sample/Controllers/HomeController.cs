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
        public ActionResult Index()
        {
            return View(new DefaultViewModel<IPageModel>(_model,_structureInfo));
        }
        public HomeController(IPageModel model, IStructureInfo structureInfo) {
            _model = model;
            _structureInfo = structureInfo;
        }
    }
}
