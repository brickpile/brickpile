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
using BrickPile.Domain.Models;
using BrickPile.UI.Models;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.UI.Controllers {
   [Authorize]
    public class ContentController : Controller {
        private dynamic _model;
        private readonly IDocumentSession _session;
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns>
        /// Returns a list of children to the current page
        /// </returns>
        public ActionResult Index() {
            if(_model != null) {
                var id = (string)_model.Id;
                var parentId = _model.Parent != null ? (string) _model.Parent.Id : null;
                var viewModel = new IndexViewModel
                                    {
                                        RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                                        CurrentModel = _model,
                                        ParentModel = parentId != null ? _session.Load<IPageModel>(parentId) : null,
                                        Children = _session.Query<IPageModel>()
                                            .Where(model => model.Parent.Id == id)
                                            .Where(model => !model.Metadata.IsDeleted)
                                            .OrderBy(model => model.Metadata.SortOrder)
                                            .ToList()
                                    };
                ViewBag.Class = "content";
                return View("Index", viewModel);
            }

            ViewBag.Class = "start";
            return View("Start", new NewModel());
        }
        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit() {
            var parentId = _model.Parent != null ? (string) _model.Parent.Id : null;
            var viewModel = new EditViewModel
                                {
                                    RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                                    CurrentModel = _model,
                                    ParentModel = parentId != null ? _session.Load<IPageModel>(parentId) : null,
                                };
            ViewBag.Class = "edit";
            return View(viewModel);
        }
        /// <summary>
        /// Updates the specified editor model.
        /// </summary>
        /// <param name="editorModel">The editor model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update(dynamic editorModel) {

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
            _model.Metadata.Published = _model.Metadata.IsPublished ? DateTime.Now : default(DateTime?);
            _model.Metadata.ChangedBy = HttpContext.User.Identity.Name;

            _session.SaveChanges();
            _session.Advanced.Refresh(_model);

            var page = _model as IPageModel;

            if (page.Parent != null) {
                _model = _session.Load<IPageModel>(page.Parent.Id);
            }

            return RedirectToAction("index", new { model = _model });
        }
        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id) {
            var model = _session.Load<IPageModel>(id.Replace("_", "/"));
            model.Metadata.IsDeleted = true;
            model.Metadata.Published = default(DateTime?);
            model.Metadata.IsPublished = false;
            _session.SaveChanges();

            ViewBag.Heading = "Delete succeeded";
            ViewBag.Message = "The page was successfully deleted";

            return PartialView("Growl", ViewBag);
        }
        /// <summary>
        /// Responsible for providing the add page view with data
        /// </summary>
        /// <returns></returns>
        public ActionResult Add() {
            return PartialView(new NewModel { CurrentModel = _model });
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
                if (page == null) {
                    throw new BrickPileException("The selected page model is not valid!");
                }
                page.Metadata.Url = parent != null ? VirtualPathUtility.AppendTrailingSlash(parent.Metadata.Url) : String.Empty;

                var viewModel = new NewPageViewModel
                                    {
                                        RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                                        CurrentModel = parent,
                                        NewPageModel = page,
                                    };

                ViewBag.Class = "edit";
                return View("new", viewModel);
            }

            return PartialView("add", newModel);
        }
        /// <summary>
        /// Saves the specified new page model.
        /// </summary>
        /// <param name="newPageModel">The new page model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Save(dynamic newPageModel) {

            if (ModelState.IsValid) {
                var parent = _model as PageModel;
                // create a new page from the new model
                var page = Activator.CreateInstance(Type.GetType(Request.Form["AssemblyQualifiedName"])) as dynamic;
                if (page == null) {
                    throw new BrickPileException("The selected page model is not valid!");
                }
                // Update all values
                UpdateModel(page, "NewPageModel");
                // Store the new page
                _session.Store(page);
                // Set the parent if it's not the start page
                if (parent != null) {
                    page.Parent = _model;
                    parent.Children.Add(page.Id);
                }
                // Set changed date
                page.Metadata.Changed = DateTime.Now;
                page.Metadata.ChangedBy = HttpContext.User.Identity.Name;

                // Add page to repository and save changes
                _session.SaveChanges();
                //_repository.Refresh(model);

                return RedirectToAction("index", new { model = parent ?? page });
            }

            return null;
        }
        /// <summary>
        /// Views the deleted.
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowDeleted() {
            var id = (string)_model.Id;
            var parentId = _model.Parent != null ? (string)_model.Parent.Id : null;
            var viewModel = new IndexViewModel
            {
                RootModel = _session.Query<IPageModel>().SingleOrDefault(model => model.Parent == null),
                CurrentModel = _model,
                ParentModel = parentId != null ? _session.Load<IPageModel>(parentId) : null,
                Children = _session.Query<IPageModel>()
                    .Where(model => model.Parent.Id == id)
                    .OrderBy(model => model.Metadata.SortOrder)
                    .ToList()
            };
            ViewBag.Class = "content";
            return View("Index", viewModel);
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
            model.Metadata.Published = published ? DateTime.Now : default(DateTime?);
            model.Metadata.Changed = DateTime.Now;
            model.Metadata.ChangedBy = HttpContext.User.Identity.Name;
            _session.SaveChanges();

            ViewBag.Heading = published ? "Publish succeeded" : "Unpublish succeeded";
            ViewBag.Message = published ? "The page was successfully published" : "The page was successfully unpublished";

            return PartialView("Growl", ViewBag);
        }
        /// <summary>
        /// Permanents the delete.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PermanentDelete(string id) {
            var model = _session.Load<IPageModel>(id.Replace("_", "/"));
            _session.Delete(model);
            _session.SaveChanges();

            ViewBag.Heading = "Permanent delete succeeded";
            ViewBag.Message = "The page was successfully permanent deleted";

            return PartialView("Growl", ViewBag);
        }
        /// <summary>
        /// Uns the delete.
        /// </summary>
        /// <returns></returns>
        public ActionResult UnDelete() {

            _model.Metadata.IsDeleted = false;
            _session.SaveChanges();

            var model = _model as IPageModel;
            IPageModel parent = null;
            if(model != null) {
                parent = _session.Load<IPageModel>(model.Parent.Id);
            }

            return RedirectToAction("showdeleted", new { model = parent ?? _model });
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
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        public ContentController(IPageModel model, IDocumentSession session) {
            _model = model;
            _session = session;
        }
    }
}
