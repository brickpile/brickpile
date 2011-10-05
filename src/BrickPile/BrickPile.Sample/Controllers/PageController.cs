using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.Infrastructure.Common;
using BrickPile.Domain.Models;
using BrickPile.Sample.Models;
using BrickPile.UI;
using BrickPile.UI.Common;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.Sample.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageModel _model;
        private readonly IStructureInfo _structureInfo;
        private readonly IDocumentSession _session;
        public ActionResult Index() {

            _structureInfo.Hierarchy = _session
                .LoadFrom<IPageModel>(x => x.Id == _model.Id)
                //.Where(x => x.Metadata.IsPublished == true)
                .OrderBy(x => x.Metadata.SortOrder)
                .AsHierarchy();

            return View(new DefaultViewModel<Page>(_model as Page, _structureInfo));
        }
        
        public PageController(IPageModel model, IStructureInfo structureInfo, IDocumentSession session) {
            _model = model;
            _structureInfo = structureInfo;
            _session = session;
        }
    }
}
