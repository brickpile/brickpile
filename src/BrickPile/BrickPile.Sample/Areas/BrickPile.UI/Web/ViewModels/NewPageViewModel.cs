using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.ViewModels {
    public class NewPageViewModel {
        /// <summary>
        /// Gets or sets the root model.
        /// </summary>
        /// <value>
        /// The root model.
        /// </value>
        public IPageModel RootModel { get; set; }
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