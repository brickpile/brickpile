using System;
using System.Web.Mvc;
using Dashboard.Models;
using Dashboard.Web.Mvc.ViewModels;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;

namespace Dashboard.Controllers {

    public class PagesController : Controller {
        private readonly IRepository _repository;
        /* *******************************************************************
        * Properties
        * *******************************************************************/
        #region public dynamic CurrentPage
        /// <summary>
        /// Gets the CurrentPage of the PagesController
        /// </summary>
        /// <value></value>
        public dynamic CurrentPage
        {
            get { return RouteData.DataTokens[DocumentRoute.DocumentKey]; }
        }
        #endregion
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public PagesController(IRepository repository)
        /// <summary>
        /// Initializes a new instance of the <b>PagesController</b> class.
        /// </summary>
        /// <param name="repository"></param>
        public PagesController(IRepository repository)
        {
            _repository = repository;
        }
        #endregion
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region public ActionResult Index()
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion
        #region public ActionResult Edit()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            var model = new DashboardViewModel(CurrentPage, _repository);
            return View(model);
        }
        #endregion
        #region public ActionResult Add()
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View("add", new DashboardViewModel(CurrentPage, _repository));
        }
        #endregion
        #region public virtual ActionResult Update([Bind(Prefix = "CurrentDocument")] dynamic currentItem)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentItem"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Update([Bind(Prefix = "CurrentDocument")] dynamic currentItem)
        {
            if (ModelState.IsValid)
            {
                UpdateModel(CurrentPage, "CurrentDocument");
                _repository.SaveChanges();

                return RedirectToAction("edit", new { document = CurrentPage });
            }
            return View("edit", new DashboardViewModel(CurrentPage, _repository));
        }
        #endregion
        #region public ActionResult Create([Bind(Prefix = "NewPageModel")] NewPageModel newPageModel)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPageModel"></param>
        /// <returns></returns>
        public ActionResult Create([Bind(Prefix = "NewPageModel")] NewPageModel newPageModel)
        {
            if (ModelState.IsValid)
            {
                // create a new page from the selected page model
                var page = Activator.CreateInstance(Type.GetType(newPageModel.SelectedPageModel)) as Document;
                // handle this gracefully in the future
                if (page == null)
                {
                    throw new Exception("The selected page model is not valid!");
                }
                // add the current page as a parent, the children of the current page is updated in the trigger
                page.Parent = CurrentPage;

                UpdateModel(page, "NewPageModel");
                _repository.Store(page);
                _repository.SaveChanges();

                return RedirectToAction("edit", new { document = page });
            }

            return View("add", new DashboardViewModel(newPageModel, _repository));
        }
        #endregion
        public ActionResult Delete() {
            _repository.Delete(CurrentPage);
            _repository.SaveChanges();
            return View("index");
        }
    }
}
