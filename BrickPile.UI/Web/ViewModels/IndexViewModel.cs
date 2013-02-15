using System.Collections.Generic;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.ViewModels {
    public class IndexViewModel {
        /// <summary>
        /// Gets or sets the root model.
        /// </summary>
        /// <value>
        /// The root model.
        /// </value>
        public IPageModel RootModel { get; set; }
        /// <summary>
        /// Gets or sets the parent model.
        /// </summary>
        /// <value>
        /// The parent model.
        /// </value>
        public IPageModel ParentModel { get; set; }
        /// <summary>
        /// Gets the current model.
        /// </summary>
        public IPageModel CurrentModel { get; set; }
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public IEnumerable<IPageModel> Children { get; set; }

    }
}