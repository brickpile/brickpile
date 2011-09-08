using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.ViewModels {
    public class NewPageViewModel : IDashboardViewModel {
        /// <summary>
        /// Get/Sets the StructureInfo of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public virtual IStructureInfo StructureInfo { get; set; }
        /// <summary>
        /// Get/Sets the CurrentModel of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public virtual IPageModel CurrentModel { get; set; }
        /// <summary>
        /// Gets or sets the new page model.
        /// </summary>
        /// <value>
        /// The new page model.
        /// </value>
        public virtual IPageModel NewPageModel { get; set; }
    }
}