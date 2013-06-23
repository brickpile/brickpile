using System;
using System.Collections.Generic;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.ViewModels {
    [Obsolete]
    public class EditViewModel {
        /// <summary>
        /// Gets or sets the root model.
        /// </summary>
        /// <value>
        /// The root model.
        /// </value>
        public IPage RootModel { get; set; }
        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        /// <value>
        /// The current model.
        /// </value>
        public IPage CurrentModel { get; set; }
        /// <summary>
        /// Gets or sets the parent model.
        /// </summary>
        /// <value>
        /// The parent model.
        /// </value>
        public IPage ParentModel { get; set; }

        /// <summary>
        /// Gets or sets the illigal slugs.
        /// </summary>
        /// <value>
        /// The illigal slugs.
        /// </value>
        public string IlligalSlugs { get; set; }
    }
}