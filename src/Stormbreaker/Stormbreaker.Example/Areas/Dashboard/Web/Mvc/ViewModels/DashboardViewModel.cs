using System.Collections.Generic;
using Dashboard.Models;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Dashboard.Web.Mvc.ViewModels {
    public class DashboardViewModel : IDashboardViewModel {
        private readonly IPageRepository _repository;
        /// <summary>
        /// Get/Sets the StructureInfo of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public virtual IStructureInfo StructureInfo { get; private set; }
        /// <summary>
        /// Get/Sets the CurrentModel of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public virtual IPageModel CurrentModel { get; private set; }
        /// <summary>
        /// Gets the page selector model.
        /// </summary>
        public virtual IList<IPageModel> PageSelectionModel {
            get {
                if (_pageSelectionModel == null) {
                    _pageSelectionModel = new List<IPageModel>();
                    _pageSelectionModel.AddRange(_repository.GetAllPages());
                }
                return _pageSelectionModel;
            }
        }
        private List<IPageModel> _pageSelectionModel;
        /// <summary>
        /// Gets the new page model.
        /// </summary>
        public NewPageModel NewPageModel { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        /// <param name="repository">The repository.</param>
        public DashboardViewModel(IPageModel model, IStructureInfo structureInfo, IPageRepository repository)
        {
            _repository = repository;
            CurrentModel = model;
            StructureInfo = structureInfo;
            NewPageModel = new NewPageModel();
        }
    }
}