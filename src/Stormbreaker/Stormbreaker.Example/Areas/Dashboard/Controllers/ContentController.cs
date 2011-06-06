using System;
using System.Web;
using System.Web.Mvc;
using Stormbreaker.Dashboard.Models;
using Stormbreaker.Dashboard.Web.Mvc.ViewModels;
using Stormbreaker.Exceptions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Dashboard.Controllers {
    [Authorize]
    public class ContentController : Controller {
        private readonly IRepository<IPageModel> _repository;
        private readonly IStructureInfo _structureInfo;
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns>
        /// Redirects to the Edit action with the home page loaded
        /// </returns>
        public ActionResult Index(dynamic model) {
            if(model != null && model is IPageModel) {
                return RedirectToAction("edit", new { model });   
            }
            return View(new DefaultPageModel());
        }
        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Edit(dynamic model) {
            var viewModel = new DashboardViewModel(model, _structureInfo);
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
            _repository.SaveChanges();

            _repository.Refresh(model);

            return RedirectToAction("edit", new { model });
        }
        /// <summary>
        /// Responsible for providing the add page view with data
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Add(dynamic model) {
            return PartialView(new CreateNewModel
                                   {
                                       CurrentModel = model,
                                       BackAction = "edit",
                                       Url = VirtualPathUtility.AppendTrailingSlash(model.Metadata.Url)
                                   });
        }
        /// <summary>
        /// Responsible for creating a new page based on the selected page model
        /// </summary>
        /// <param name="newPageModel">The new page model.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Create(CreateNewModel newPageModel, dynamic model) {
            var parent = model as IPageModel;
            if(parent == null) {
                throw new StormbreakerException("The injected model is not a PageModel");
            }

            if (ModelState.IsValid) {
                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newPageModel.SelectedPageModel)) as IPageModel;
                // handle this gracefully in the future :)
                if (page == null) {
                    throw new StormbreakerException("The selected page model is not valid!");
                }

                page.Parent = model;
                page.Metadata.Name = newPageModel.Name;
                page.Metadata.Slug = newPageModel.Slug;
                page.Metadata.Url = newPageModel.Url;

                _repository.Store(page);
                _repository.SaveChanges();
                _repository.Refresh(page);

                return RedirectToAction("edit", new { model = page });
            }
            return PartialView("add", newPageModel);
        }
        /// <summary>
        /// Creates the default.
        /// </summary>
        /// <param name="defaultPageModel">The default page model.</param>
        /// <returns></returns>
        public ActionResult CreateDefault(DefaultPageModel defaultPageModel) {
            if(ModelState.IsValid) {
                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(defaultPageModel.SelectedPageModel)) as IPageModel;
                // handle this gracefully in the future :)
                if (page == null) {
                    throw new StormbreakerException("The selected page model is not valid!");
                }
                page.Metadata.Name = defaultPageModel.Name;
                _repository.Store(page);
                _repository.SaveChanges();

                return RedirectToAction("edit", new { model = page });
            }
            return View("index", defaultPageModel);
        }
        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(IPageModel model) {
            return PartialView("Confirm", new ConfirmFormModel()
                                       {
                                           BackAction = "edit",
                                           CurrentModel = model                                           
                                       } );
        }
        /// <summary>
        /// Pastes the specified source id.
        /// </summary>
        /// <param name="sourceId">The source id.</param>
        /// <param name="destinationId">The destination id.</param>
        /// <returns></returns>
        public ActionResult Paste(string sourceId, string destinationId) {

            dynamic source = _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(sourceId));
            dynamic destination = _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(destinationId));

            source.Parent = destination;
            _repository.SaveChanges();

            return RedirectToAction("Index");

        }
        /// <summary>
        /// Deletes the specified confirm form model.
        /// </summary>
        /// <param name="confirmFormModel">The confirm form model.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(ConfirmFormModel confirmFormModel, IPageModel model) {

            _repository.Delete(model);
            _repository.SaveChanges();

            if (model.Parent != null) {
                var parent = _repository.SingleOrDefault<IPageModel>(x => x.Id.Equals(model.Parent.Id));
                return RedirectToAction("edit", new { model = parent });
            }
            return RedirectToAction("index");            
        }
        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="structureInfo">The structure info.</param>
        public ContentController(IRepository<IPageModel> repository, IStructureInfo structureInfo) {
            _repository = repository;
            _structureInfo = structureInfo;
        }
    }
}
