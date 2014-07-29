using System.Collections.Generic;
using IPage = BrickPile.Core.IPage;

namespace BrickPile.UI.Web.ViewModels {
    public class IndexViewModel {
        /// <summary>
        /// Gets or sets the root model.
        /// </summary>
        /// <value>
        /// The root model.
        /// </value>
        public IPage RootModel { get; set; }
        /// <summary>
        /// Gets or sets the parent model.
        /// </summary>
        /// <value>
        /// The parent model.
        /// </value>
        public IPage ParentModel { get; set; }
        /// <summary>
        /// Gets the current model.
        /// </summary>
        public IPage CurrentModel { get; set; }
        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public IEnumerable<IPage> Children { get; set; }

        public IEnumerable<string> Ids { get; set; } 
    }
}