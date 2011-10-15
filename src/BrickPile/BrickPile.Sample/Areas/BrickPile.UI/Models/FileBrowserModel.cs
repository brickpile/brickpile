using System.Collections.Generic;

namespace BrickPile.UI.Models {
    public class FileBrowserModel {
        /// <summary>
        /// Gets or sets the back action.
        /// </summary>
        /// <value>
        /// The back action.
        /// </value>
        public string BackAction { get; set; }
        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        /// <value>
        /// The assets.
        /// </value>
        public IList<AssetModel> Assets { get; set; }
    }
}