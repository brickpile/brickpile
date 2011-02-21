using System;
using System.Web.Mvc;
using Dashboard.Models;
using Dashboard.Web.Mvc.ViewModels;
using Stormbreaker.Configuration;
using Stormbreaker.Exceptions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Dashboard.Controllers {
    public class ContentController : Controller {
        private readonly IPageRepository _repository;
        private readonly IStructureInfo _structureInfo;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns>
        /// Redirects to the Edit action with the home page loaded
        /// </returns>
        public ActionResult Index() {
            return RedirectToAction("edit", new { model = _repository.Load<dynamic>(_configuration.HomePageId)}); 
        }
        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Edit(dynamic model)
        {
            var viewModel = new DashboardViewModel(model, _structureInfo,_repository);
            return View(viewModel);
        }
        /// <summary>
        /// Responsible for saving all changes made to the current page
        /// </summary>
        /// <param name="editPageModel">The edit page model.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update([Bind(Prefix = "CurrentModel")] dynamic editPageModel, dynamic model)
        {
            if (ModelState.IsValid)
            {
                UpdateModel(model, "CurrentModel");
                _repository.SaveChanges();

                return RedirectToAction("edit", new { model });
            }
            return View("edit", new DashboardViewModel(model, _structureInfo,_repository));
        }
        /// <summary>
        /// Responsible for providing the add page view with data
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Add(dynamic model)
        {
            return View("add", new DashboardViewModel(model, _structureInfo,_repository));
        }
        /// <summary>
        /// Responsible for creating a new page based on the selected page model
        /// </summary>
        /// <param name="newPageModel">The new page model.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Create([Bind(Prefix = "NewPageModel")] NewPageModel newPageModel, dynamic model)
        {
            if (ModelState.IsValid)
            {
                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newPageModel.SelectedPageModel)) as PageModel;
                // handle this gracefully in the future
                if (page == null)
                {
                    throw new StormbreakerException("The selected page model is not valid!");
                }
                // add the current page as a parent, the children of the current page is updated in the trigger
                page.Parent = model;

                UpdateModel(page, "NewPageModel");
                _repository.Store(page);
                _repository.SaveChanges();

                return RedirectToAction("edit", new { model = page });
            }

            return View("add", new DashboardViewModel(newPageModel, _structureInfo,_repository));
        }
        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Delete(dynamic model)
        {
            _repository.Delete(model);
            _repository.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="configuration">The configuration.</param>
        public ContentController(IPageRepository repository, IStructureInfo structureInfo, IConfiguration configuration)
        {
            _repository = repository;
            _structureInfo = structureInfo;
            _configuration = configuration;
        }
    }
}
