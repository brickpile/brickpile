using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Core.Extensions;
using BrickPile.Core.Infrastructure.Indexes;
using BrickPile.Core.Mvc;
using BrickPile.UI.Areas.UI.Models;
using BrickPile.UI.Web.Mvc;
using BrickPile.UI.Web.ViewModels;
using Newtonsoft.Json;
using Raven.Client;

namespace BrickPile.UI.Areas.UI.Controllers
{
    [Authorize, EditorControls(Disable = true)]
    public class PagesController : Controller
    {
        private readonly IDocumentStore documentStore;

        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="documentStore">The documentStore.</param>
        public PagesController(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        /// <summary>
        ///     Default action
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <param name="deleted">if set to <c>true</c> [deleted].</param>
        /// <returns>
        ///     Returns a list of children to the current page
        /// </returns>
        public ActionResult Index(dynamic currentPage, bool deleted = false) {
            // If the current page is null, assume we haven't created the start page yet
            if (!(currentPage is Page))
            {
                ViewBag.Class = "start";
                return View("Start", new NewModel());
            }

            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var viewModel = new IndexViewModel
                {
                    RootModel = session.Advanced.GetStartPage(),
                    CurrentModel = currentPage,
                    ParentModel = session.Advanced.GetParentFor<IPage>((IPage) currentPage),
                    Children = session.Advanced.GetChildrenFor((IPage) currentPage)
                };
                return View("Index", viewModel);
            }
        }

        /// <summary>
        ///     Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(IPage currentPage) {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var viewModel = new EditViewModel
                {
                    RootModel = session.Advanced.GetStartPage(),
                    CurrentModel = session.Advanced.GetDraftFor<IPage>(currentPage) ?? currentPage,
                    ParentModel = session.Advanced.GetParentFor<IPage>(currentPage),
                    IlligalSlugs = currentPage.Parent != null
                        ? JsonConvert.SerializeObject(
                            session.Advanced.GetChildrenFor(currentPage)
                                .Select(x => x.Metadata.Slug))
                        : null
                };

                ViewBag.Class = "edit";
                return View(viewModel);
            }
        }

        /// <summary>
        ///     Previews the specified current page.
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public ActionResult Preview(IPage currentPage) {
            if (!TempData.ContainsKey("EditoAction"))
            {
                TempData["EditorAction"] = EditorAction.Preview;
            }            
            return
                new RedirectResult(string.Concat("~/", VirtualPathUtility.AppendTrailingSlash(currentPage.Metadata.Url)));
        }

        /// <summary>
        ///     Updates the specified editor model.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update(dynamic currentPage, FormCollection collection) {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                if (!TryUpdateModel(currentPage, "CurrentModel", collection))
                {
                    var viewModel = new EditViewModel
                    {
                        RootModel = session.Advanced.GetStartPage(),
                        CurrentModel = currentPage,
                        ParentModel = session.Advanced.GetParentFor<IPage>((IPage) currentPage),
                    };

                    return View("edit", viewModel);
                }

                session.Store((IPage) currentPage,
                    currentPage.Metadata.IsPublished ? StoreAction.Publish : StoreAction.Save);
                session.SaveChanges();

                if (currentPage.Parent != null)
                {
                    currentPage = session.Advanced.GetParentFor<IPage>((IPage) currentPage);
                }

                return RedirectToAction("index", new {currentPage});
            }
        }

        /// <summary>
        ///     Deletes the specified model.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="permanent">if set to <c>true</c> [permanent].</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id, bool permanent) {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var model = session.Load<IPage>(id.Replace("_", "/"));

                if (permanent)
                {
                    session.Delete(model);
                }
                else
                {
                    model.Metadata.IsDeleted = true;
                }
                session.SaveChanges();
            }

            return new EmptyResult();
        }

        /// <summary>
        ///     Restores the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult Restore(string id) {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var model = session.Load<IPage>(id.Replace("_", "/"));
                model.Metadata.IsDeleted = false;
                session.SaveChanges();
            }
            return new EmptyResult();
        }

        /// <summary>
        ///     News the specified new page model.
        /// </summary>
        /// <param name="newModel">The new model.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        public ActionResult New(NewModel newModel, dynamic currentPage) {
            if (ModelState.IsValid)
            {
                var parent = currentPage as IPage;

                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newModel.SelectedPageModel)) as IPage;

                using (IDocumentSession session = this.documentStore.OpenSession())
                {
                    var viewModel = new NewPageViewModel
                    {
                        RootModel = page,
                        ParentModel = parent,
                        NewPageModel = page,
                        SlugsInUse =
                            parent != null
                                ? JsonConvert.SerializeObject(
                                    session.Advanced.GetChildrenFor(parent).Select(x => x.Metadata.Slug))
                                : null
                    };

                    ViewBag.Class = "edit";
                    return View("new", viewModel);
                }
            }

            return PartialView("add", newModel);
        }

        /// <summary>
        ///     Saves the specified new page model.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Save(
            [ModelBinder(typeof (ContentModelBinder)), Bind(Prefix = "NewPageModel")] IPage pageModel,
            dynamic currentPage) {
            var parent = currentPage as Page;

            if (!ModelState.IsValid)
            {
                NewPageViewModel viewModel;
                using (IDocumentSession session = this.documentStore.OpenSession())
                {
                    viewModel = new NewPageViewModel
                    {
                        RootModel = session.Advanced.GetStartPage(),
                        ParentModel = parent,
                        NewPageModel = pageModel,
                        SlugsInUse =
                            parent != null
                                ? JsonConvert.SerializeObject(
                                    session.Advanced.GetChildrenFor((IPage) currentPage)
                                        .Select(x => x.Metadata.Slug))
                                : null
                    };
                }

                return View("new", viewModel);
            }

            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                session.Store(pageModel, pageModel.Metadata.IsPublished ? StoreAction.Publish : StoreAction.Save);

                if (parent != null)
                {
                    pageModel.Parent = (Page) currentPage;

                    IPage[] children = session.Advanced.GetChildrenFor((IPage) currentPage).ToArray();

                    int max = children.Length > 0 ? children.Max(x => x.Metadata.SortOrder) : 0;

                    pageModel.Metadata.SortOrder = max + 1;
                }

                session.SaveChanges();
            }
            return RedirectToAction("index", new {currentPage = parent ?? pageModel});
        }

        /// <summary>
        ///     Publishes this instance.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="published">if set to <c>true</c> [published].</param>
        /// <returns></returns>
        public virtual ActionResult Publish(string id, bool published) {
            string identity = id.Replace("_", "/");

            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var model = session.Load<IPage>(identity);
                session.Store(model, published ? StoreAction.Publish : StoreAction.UnPublish);
                session.SaveChanges();
            }

            return new EmptyResult();
        }

        /// <summary>
        ///     Sorts the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void Sort(List<string> items) {
            // replace all underscore with slash
            string[] ids = items.Select(key => key.Replace("_", "/")).ToArray();
            // load all documents
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                IPage[] documents = session.Load<IPage>(ids.ToArray());

                int order = 1;
                foreach (IPage model in documents)
                {
                    model.Metadata.SortOrder = order++;
                }

                session.SaveChanges();
            }
        }

        /// <summary>
        ///     Searches the specified term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Search(string term) {
            object r;
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                IQueryable<IPage> result = from page in session.Query<IPage, AllPages>()
                    where page.Metadata.Name.StartsWith(term)
                    select page;

                r = from p in result.ToList()
                    orderby p.Metadata.Name ascending
                    select new {id = p.Id, value = p.Metadata.Name, label = p.Metadata.Name};
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Discards the draft and returns the main document.
        /// </summary>
        /// <returns></returns>
        public ActionResult Discard(dynamic currentPage) {
            using (IDocumentSession session = this.documentStore.OpenSession())
            {
                var draft = session.Advanced.GetDraftFor<IPage>((IPage) currentPage);
                session.Delete(draft);
                session.SaveChanges();

                return RedirectToAction("edit", new {currentPage});
            }
        }
    }
}