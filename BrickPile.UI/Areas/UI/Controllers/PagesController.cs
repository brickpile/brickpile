using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Core.Extensions;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.UI.Areas.UI.Models;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.ViewModels;
using Raven.Client;

namespace BrickPile.UI.Areas.UI.Controllers {
    [Authorize]
    public class PagesController : Controller {
        private dynamic _currentPage;
        private readonly IDocumentStore _store;

        /// <summary>
        /// Default action
        /// </summary>
        /// <param name="deleted">if set to <c>true</c> [deleted].</param>
        /// <returns>
        /// Returns a list of children to the current page
        /// </returns>
        public ActionResult Index(bool deleted = false) {
            // If the current page is null, assume we haven't created the start page yet
            if (_currentPage == null) {
                ViewBag.Class = "start";
                return View("Start", new NewModel());
            }

            using (var session = _store.OpenSession()) {
                var viewModel = new IndexViewModel
                {
                    RootModel = session.Advanced.GetStartPage(),
                    CurrentModel = _currentPage,
                    ParentModel = session.Advanced.GetParentFor((IPage) _currentPage),
                    Children = session.Advanced.GetChildrenFor<IPage>((IPage) _currentPage)
                };
                return View("Index", viewModel);
            }
        }

        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit() {
            using (var session = _store.OpenSession()) {
                var viewModel = new EditViewModel
                {
                    RootModel = session.Advanced.GetStartPage(),
                    CurrentModel = _currentPage,
                    ParentModel = session.Advanced.GetParentFor((IPage) _currentPage),
                    IlligalSlugs = _currentPage.Parent != null
                        ? Newtonsoft.Json.JsonConvert.SerializeObject(
                            session.Advanced.GetChildrenFor<IPage>((IPage) _currentPage)
                                .Select(x => x.Metadata.Slug))
                        : null
                };

                ViewBag.Class = "edit";
                return View(viewModel);
            }
        }

        /// <summary>
        /// Updates the specified editor model.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update(FormCollection collection) {
            using (var session = _store.OpenSession()) {
                if (!TryUpdateModel(_currentPage, "CurrentModel", collection)) {
                    var viewModel = new EditViewModel
                    {
                        RootModel = session.Advanced.GetStartPage(),
                        CurrentModel = _currentPage,
                        ParentModel = session.Advanced.GetParentFor((IPage) _currentPage),
                    };

                    return View("edit", viewModel);
                }

                session.Store((IPage)_currentPage, _currentPage.Metadata.IsPublished ? StoreAction.Publish : StoreAction.Save);
                session.SaveChanges();

                if (_currentPage.Parent != null) {
                    _currentPage = session.Advanced.GetParentFor((IPage) _currentPage);
                }

                return RedirectToAction("index", new { currentPage = _currentPage });
            }
        }

        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="permanent">if set to <c>true</c> [permanent].</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id, bool permanent) {
            using (var session = _store.OpenSession()) {

                var model = session.Load<IPage>(id.Replace("_", "/"));

                if (permanent) {

                    session.Delete(model);

                }
                else {

                    model.Metadata.IsDeleted = true;

                }
                session.SaveChanges();
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Restores the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult Restore(string id) {
            using (var session = _store.OpenSession()) {
                var model = session.Load<IPage>(id.Replace("_", "/"));
                model.Metadata.IsDeleted = false;
                session.SaveChanges();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// News the specified new page model.
        /// </summary>
        /// <param name="newModel">The new model.</param>
        /// <returns></returns>
        public ActionResult New(NewModel newModel) {
            if (ModelState.IsValid) {
                var parent = _currentPage as IPage;

                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newModel.SelectedPageModel)) as IPage;

                using (var session = _store.OpenSession()) {
                    var viewModel = new NewPageViewModel
                    {
                        RootModel = page,
                        ParentModel = parent,
                        NewPageModel = page,
                        SlugsInUse =
                            parent != null
                                ? Newtonsoft.Json.JsonConvert.SerializeObject(
                                    session.Advanced.GetChildrenFor<IPage>(parent).Select(x => x.Metadata.Slug))
                                : null
                    };

                    ViewBag.Class = "edit";
                    return View("new", viewModel);
                }
            }

            return PartialView("add", newModel);
        }

        /// <summary>
        /// Saves the specified new page model.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Save(
            [ModelBinder(typeof (ContentModelBinder)), Bind(Prefix = "NewPageModel")] IPage pageModel) {
            var parent = _currentPage as Page;

            if (!ModelState.IsValid) {
                NewPageViewModel viewModel;
                using (var session = _store.OpenSession()) {
                    viewModel = new NewPageViewModel
                    {
                        RootModel = session.Advanced.GetStartPage(),
                        ParentModel = parent,
                        NewPageModel = pageModel,
                        SlugsInUse =
                            parent != null
                                ? Newtonsoft.Json.JsonConvert.SerializeObject(
                                    session.Advanced.GetChildrenFor<Page>((IPage) _currentPage)
                                        .Select(x => x.Metadata.Slug))
                                : null
                    };
                }

                return View("new", viewModel);
            }

            using (var session = _store.OpenSession()) {
                session.Store(pageModel, StoreAction.Save);

                if (parent != null) {
                    pageModel.Parent = (Page) _currentPage;

                    var children = session.Advanced.GetChildrenFor<Page>((IPage) _currentPage).ToArray();

                    var max = children.Length > 0 ? children.Max(x => x.Metadata.SortOrder) : 0;

                    pageModel.Metadata.SortOrder = max + 1;
                }

                session.SaveChanges();
            }
            return RedirectToAction("index", new {currentPage = parent ?? pageModel});
        }

        /// <summary>
        /// Publishes this instance.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="published">if set to <c>true</c> [published].</param>
        /// <returns></returns>
        public virtual ActionResult Publish(string id, bool published) {
            var identity = id.Replace("_", "/");

            using (var session = _store.OpenSession()) {
                var model = session.Load<IPage>(identity);
                session.Store(model, published ? StoreAction.Publish : StoreAction.UnPublish);
                session.SaveChanges();
            }

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
            using (var session = _store.OpenSession()) {
                var documents = session.Load<IPage>(ids.ToArray());

                var order = 1;
                foreach (var model in documents) {
                    model.Metadata.SortOrder = order++;
                }

                session.SaveChanges();
            }
        }

        /// <summary>
        /// Searches the specified term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Search(string term) {
            object r;
            using (var session = _store.OpenSession()) {
                var result = from page in session.Query<IPage, AllPages>()
                    where page.Metadata.Name.StartsWith(term)
                    select page;

                r = from p in result.ToList()
                    orderby p.Metadata.Name ascending
                    select new {id = p.Id, value = p.Metadata.Name, label = p.Metadata.Name};
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Discards the draft and returns the main document.
        /// </summary>
        /// <returns></returns>
        public ActionResult Discard() {
            var id = _currentPage.Id;
            using (var session = _store.OpenSession()) {
                var draft = session.Load<IPage>(id);
                session.Delete(draft);
                session.SaveChanges();

                var currentPage = session.Load<IPage>(id.Replace("/draft", ""));

                return RedirectToAction("edit", new {currentPage});
            }
        }

        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="store">The store.</param>
        public PagesController(IPage currentPage, IDocumentStore store) {
            _currentPage = currentPage;
            _store = store;
        }
    }
}