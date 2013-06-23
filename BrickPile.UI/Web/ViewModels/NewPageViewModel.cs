using System;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.ViewModels {
    [Obsolete]
    public class NewPageViewModel {
        /// <summary>
        /// Gets or sets the root model.
        /// </summary>
        /// <value>
        /// The root model.
        /// </value>
        public IPage RootModel { get; set; }
        /// <summary>
        /// Get/Sets the CurrentModel of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public IPage ParentModel { get; set; }
        /// <summary>
        /// Gets or sets the new page model.
        /// </summary>
        /// <value>
        /// The new page model.
        /// </value>
        public Page NewPageModel { get; set; }

        /// <summary>
        /// Gets or sets the slugs in use.
        /// </summary>
        /// <value>
        /// The slugs in use.
        /// </value>
        public string SlugsInUse { get; set; }
    }
}