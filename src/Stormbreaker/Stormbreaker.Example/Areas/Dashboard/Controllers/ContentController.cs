using System;
using System.Web.Mvc;
using Dashboard.Models;
using Dashboard.Web.Mvc.ViewModels;
using Stormbreaker.Exceptions;
using Stormbreaker.Models;
using Stormbreaker.Repositories;

namespace Dashboard.Controllers {

    public class ContentController : Controller {
        private readonly PageRepository _repository;
        /// <summary>
        /// Default action
        /// </summary>
        /// <returns>Redirects to the Edit action with the home page loaded</returns>
        public ActionResult Index() {
            return RedirectToAction("edit", new { model = _repository.Load<dynamic>("pages/1")}); 
        }
        /// <summary>
        /// Responsible for providing the Edit view with data from the current page
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(dynamic model)
        {
            var viewModel = new DashboardViewModel(model, _repository);
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
            return View("edit", new DashboardViewModel(model, _repository));
        }
        /// <summary>
        /// Responsible for providing the add page view with data
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(dynamic model)
        {
            return View("add", new DashboardViewModel(model, _repository));
        }
        /// <summary>
        /// Responsible for creating a new page based on the selected page model
        /// </summary>
        /// <param name="newPageModel"></param>
        /// /// <param name="model"></param>
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

            return View("add", new DashboardViewModel(newPageModel, _repository));
        }
        public ActionResult Delete(dynamic model)
        {
            _repository.Delete(model);
            _repository.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="repository"></param>
        public ContentController(PageRepository repository)
        {
            _repository = repository;
        }
    }
}
