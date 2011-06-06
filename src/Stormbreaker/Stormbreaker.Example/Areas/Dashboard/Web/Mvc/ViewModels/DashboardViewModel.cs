using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Dashboard.Web.Mvc.ViewModels {
    public class DashboardViewModel : IDashboardViewModel {
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
        /// Initializes a new instance of the <see cref="DashboardViewModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="structureInfo">The structure info.</param>
        public DashboardViewModel(IPageModel model, IStructureInfo structureInfo) {
            CurrentModel = model;
            StructureInfo = structureInfo;
        }
    }
}