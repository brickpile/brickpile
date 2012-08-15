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
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Domain.Models;
using BrickPile.UI.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.UI.Controllers {

   [Authorize]
    public class PagesController : Controller {
        private dynamic _model;
        private readonly IDocumentSession _session;
        /// <summary>
        /// Default action
        /// </summary>
        /// <param name="deleted">if set to <c>true</c> [deleted].</param>
        /// <returns>
        /// Returns a list of children to the current page
        /// </returns>
        public ActionResult Index(bool deleted = false) {

            // If the current page is null, assume we haven't created the start page yet
            if (_model == null) {
                ViewBag.Class = "start";
                return View("Start", new NewModel());
            }

            var id = (string)_model.Id;
            var parentId = _model.Parent != null ? (string)_model.Parent.Id : null;

            var viewModel = new IndexViewModel
                                {
                                    RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                                    CurrentModel = _model,
                                    ParentModel = parentId != null ? _session.Load<IPageModel>(parentId) : null,
                                    Children = _session.Query<IPageModel, DocumentsByParent>()
                                        .Where(model => model.Parent.Id == id)
                                        .Where(model => model.Metadata.IsDeleted == deleted)
                                        .OrderBy(model => model.Metadata.SortOrder)
                                        .ToList()
                                };
            
            return View("Index", viewModel);

        }
        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit() {
            IPageModel parent = _model.Parent != null ? _session.Load<IPageModel>(_model.Parent.Id) : null;
            var viewModel = new EditViewModel
                                {
                                    RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                                    CurrentModel = _model,
                                    ParentModel = parent,
                                    IlligalSlugs = parent != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_session.Load<IPageModel>(parent.Children).Select(x => x .Metadata.Slug)) : null
                                };

            if (Request.IsAjaxRequest()) {
                return PartialView("Edit", viewModel);
            }

            ViewBag.Class = "edit";
            return View(viewModel);
        }
        /// <summary>
        /// Updates the specified editor model.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update() {

            if (!TryUpdateModel(_model, "CurrentModel")) {
                var parentId = _model.Parent != null ? (string)_model.Parent.Id : null;
                var viewModel = new EditViewModel
                {
                    RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                    CurrentModel = _model,
                    ParentModel = parentId != null ? _session.Load<IPageModel>(parentId) : null,
                };
                return View("edit", viewModel);
            }

            UpdateModel(_model);

            _model.Metadata.Changed = DateTime.Now;
            _model.Metadata.ChangedBy = HttpContext.User.Identity.Name;

            _session.SaveChanges();

            var page = _model as IPageModel;

            if (page.Parent != null) {
                _model = _session.Load<IPageModel>(page.Parent.Id);
            }

            return RedirectToAction("index", new { model = _model });
        }
        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="permanent">if set to <c>true</c> [permanent].</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id, bool permanent) {
            
            var model = _session.Load<IPageModel>(id.Replace("_", "/"));

            if(permanent) {
                _session.Delete(model);
                if(model.Parent != null) {
                    var parent = _session.Load<IPageModel>(model.Parent.Id);
                    parent.Children.Remove(model.Id);
                }
            } else {
                model.Metadata.IsDeleted = true;
            }
            _session.SaveChanges();

            return new EmptyResult();

        }
        /// <summary>
        /// Restores the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult Restore(string id) {
            var model = _session.Load<IPageModel>(id.Replace("_", "/"));
            model.Metadata.IsDeleted = false;
            _session.SaveChanges();
            return new EmptyResult();
        }
        /// <summary>
        /// News the specified new page model.
        /// </summary>
        /// <param name="newModel">The new model.</param>
        /// <returns></returns>
        public ActionResult New(NewModel newModel) {

            if (ModelState.IsValid) {

                var parent = _model as IPageModel;
                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newModel.SelectedPageModel)) as dynamic;
                page.Metadata.Url = parent != null ? VirtualPathUtility.AppendTrailingSlash(parent.Metadata.Url) : String.Empty;
                page.Metadata.Published = DateTime.Now;

                var viewModel = new NewPageViewModel
                                    {
                                        RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                                        ParentModel = parent,
                                        NewPageModel = page,
                                        SlugsInUse = parent != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_session.Load<IPageModel>(parent.Children).Select(x => x.Metadata.Slug)) : null
                                    };
                
                ViewBag.Class = "edit";
                return View("new", viewModel);
                //return PartialView("new", viewModel);

            }

            return PartialView("add", newModel);
        }
        /// <summary>
        /// Saves the specified new page model.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Save() {

            if (ModelState.IsValid) {
                var parent = _model as PageModel;
                // create a new page from the new model
                var page = Activator.CreateInstance(Type.GetType(Request.Form["AssemblyQualifiedName"])) as dynamic;

                if(!TryUpdateModel(page,"NewPageModel")) {
                    var viewModel = new NewPageViewModel
                    {
                        RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                        ParentModel = parent,
                        NewPageModel = page,
                        SlugsInUse = parent != null ? Newtonsoft.Json.JsonConvert.SerializeObject(_session.Load<IPageModel>(parent.Children).Select(x => x.Metadata.Slug)) : null
                    };
                    return View("new", viewModel);
                }

                // Update all values
                UpdateModel(page, "NewPageModel");
                // Store the new page
                _session.Store(page);
                // Set the parent if it's not the start page
                if (parent != null) {
                    page.Parent = _model;
                    parent.Children.Add(page.Id);

                    var children = _session.Load<IPageModel>(parent.Children);
                    var max = children.Max(x => x.Metadata.SortOrder);
                    page.Metadata.SortOrder = max + 1;
                }
                // Set changed date
                page.Metadata.Changed = DateTime.Now;
                page.Metadata.ChangedBy = HttpContext.User.Identity.Name;
                
                // Add page to repository and save changes
                _session.SaveChanges();

                return RedirectToAction("index", new { model = parent ?? page });
            }

            return null;
        }
        /// <summary>
        /// Publishes this instance.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="published">if set to <c>true</c> [published].</param>
        /// <returns></returns>
        public virtual ActionResult Publish(string id, bool published) {
            var model = _session.Load<IPageModel>(id.Replace("_","/"));
            model.Metadata.IsPublished = published;
            model.Metadata.Changed = DateTime.Now;
            model.Metadata.ChangedBy = HttpContext.User.Identity.Name;
            _session.SaveChanges();

            return new EmptyResult();
        }
        /// <summary>
        /// Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Sort(List<string> items) {
            // replace all underscore with slash
            var ids = items.Select(key => key.Replace("_", "/")).ToArray();
            // load all documents
            var documents = _session.Load<IPageModel>(ids.ToArray());
            var order = 1;
            foreach (var model in documents) {
                model.Metadata.SortOrder = order++;
            }
            _session.SaveChanges();
        }
        /// <summary>
        /// Searches the specified term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Search(string term) {

            var result = from page in _session.Query<IPageModel>()
                         where page.Metadata.Name.StartsWith(term)
                         select page;

            var r = from p in result.ToList() select new { id = p.Id, value = p.Metadata.Name, label = p.Metadata.Name };

            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public PagesController(IPageModel model, IDocumentSession session) {
            _model = model;
            _session = session;
        }
    }
}
