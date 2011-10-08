/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Exception;
using BrickPile.Core.Infrastructure.Common;
using BrickPile.Core.Repositories;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using BrickPile.UI.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.UI.Controllers {
    [Authorize]
    public class ContentController : Controller {
        
        private readonly dynamic _model;
        private readonly IRepository<IPageModel> _repository;
        private readonly IDocumentSession _session;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns>
        /// Redirects to the Edit action with the home page loaded
        /// </returns>
        public ActionResult Index() {
            if (_model != null && _model is IPageModel) {
                _structureInfo.Hierarchy = _session.LoadFrom<IPageModel>(x => x.Id == _model.Id).AsHierarchy();
                var viewModel = new DashboardViewModel(_model, _structureInfo);
                ViewBag.Class = "content";
                return View("Index", viewModel);

            }
            // assume we ain't got any pages
            ViewBag.Class = "start";
            return View("Start", new NewModel());
        }
        /// <summary>
        /// Views the deleted.
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowDeleted() {
            _structureInfo.Hierarchy = _session.LoadFrom<IPageModel>(x => x.Id == _model.Id).AsHierarchy();
            var viewModel = new DashboardViewModel(_model, _structureInfo);
            ViewBag.Class = "content";
            return View("Index", viewModel);
        }

        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit() {
            var viewModel = new DashboardViewModel(_model, _structureInfo);
            ViewBag.Class = "edit";
            return View(viewModel);
        }
        /// <summary>
        /// Responsible for saving all changes made to the current page
        /// </summary>
        /// <param name="editorModel">The editor model.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update(dynamic editorModel, dynamic model) {

            if (!TryUpdateModel(model, "CurrentModel")) {
                return View("edit", new DashboardViewModel(model, _structureInfo));
            }
            
            UpdateModel(model);

            model.Metadata.Changed = DateTime.Now;
            model.Metadata.ChangedBy = HttpContext.User.Identity.Name;

            _repository.SaveChanges();
            _repository.Refresh(model);

            var page = model as IPageModel;

            if(page.Parent != null) {
                model = _repository.SingleOrDefault<IPageModel>(m => m.Id == page.Parent.Id);
            }

            return RedirectToAction("index", new { model });
        }
        /// <summary>
        /// Responsible for providing the add page view with data
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Add(dynamic model) {
            return PartialView(new NewModel { CurrentModel = model });
        }
        /// <summary>
        /// News the specified new page model.
        /// </summary>
        /// <param name="newModel">The new model.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult New(NewModel newModel, dynamic model) {
            if (ModelState.IsValid) {
                var parent = model as IPageModel;
                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newModel.SelectedPageModel)) as dynamic;
                if (page == null) {
                    throw new BrickPileException("The selected page model is not valid!");
                }
                page.Metadata.Url = parent != null ? VirtualPathUtility.AppendTrailingSlash(parent.Metadata.Url) : String.Empty;

                ViewBag.Class = "edit";
                return View("new", new NewPageViewModel { NewPageModel = page, CurrentModel = parent, StructureInfo = _structureInfo });
            }

            return PartialView("add", newModel);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Save(dynamic newPageModel, dynamic model) {

            if (ModelState.IsValid) {
                var parent = model as IPageModel;
                // create a new page from the new model
                var page = Activator.CreateInstance(Type.GetType(Request.Form["AssemblyQualifiedName"])) as dynamic;
                if (page == null) {
                    throw new BrickPileException("The selected page model is not valid!");
                }
                // Update all values
                UpdateModel(page, "NewPageModel");
                // Store the new page
                _repository.Store(page);
                // Set the parent if it's not the start page
                if(parent != null) {
                    page.Parent = model;
                    parent.Children.Add(page.Id);
                }
                // Set changed date
                page.Metadata.Changed = DateTime.Now;
                page.Metadata.ChangedBy = HttpContext.User.Identity.Name;

                // Add page to repository and save changes
                _repository.SaveChanges();
                //_repository.Refresh(model);

                return RedirectToAction("index", new { model = parent ?? page });
            }

            return null;
        }
        /// <summary>
        /// Publishes this instance.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Publish(string id, bool published) {

            var model = _repository.SingleOrDefault<IPageModel>(m => m.Id == id.Replace("_","/"));
            model.Metadata.IsPublished = published;
            model.Metadata.Changed = DateTime.Now;
            _repository.SaveChanges();

            ViewBag.Heading = published ? "Publish succeeded" : "Unpublish succeeded";
            ViewBag.Message = published ? "The page was successfully published" : "The page was successfully unpublished";

            return PartialView("Growl", ViewBag);

        }
        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id) {

            var model = _repository.SingleOrDefault<IPageModel>(m => m.Id == id.Replace("_", "/"));
            model.Metadata.IsDeleted = true;
            _repository.SaveChanges();

            ViewBag.Heading = "Delete succeeded";
            ViewBag.Message = "The page was successfully deleted";

            return PartialView("Growl", ViewBag);
        }
        [HttpPost]
        public ActionResult UnDelete(string id) {
            var model = _repository.SingleOrDefault<IPageModel>(m => m.Id == id.Replace("_", "/"));
            model.Metadata.IsDeleted = false;
            _repository.SaveChanges();

            ViewBag.Heading = "Undelete succeeded";
            ViewBag.Message = "The page was successfully undeleted";

            return PartialView("Growl", ViewBag);            
        }

        public ActionResult Preview() {
            return View(new DashboardViewModel(_model,_structureInfo));
        }
        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Sort(List<string> items) {
            // replace all underscore with slash
            var ids = items.Select(key => key.Replace("_", "/")).ToArray();
            // load all documents
            var documents = _repository.Load<IPageModel>(ids.ToArray());
            var order = 1;
            foreach (var model in documents) {
                model.Metadata.SortOrder = order++;
            }
            _repository.SaveChanges();
        }
        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="session">The session.</param>
        public ContentController(IPageModel model, IStructureInfo structureInfo, IRepository<IPageModel> repository, IDocumentSession session) {
            _model = model;
            _repository = repository;
            _session = session;
            _structureInfo = structureInfo;
        }
    }
}
