using System.Collections.Generic;
using BrickPile.Core;

namespace BrickPile.UI.Web.ViewModels {
    /// <summary>
    /// The default view model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultViewModel<T> : IViewModel<T> where T : IPage {

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public T CurrentPage { get; set; }

        /// <summary>
        /// Gets the structure info.
        /// </summary>
        public NavigationContext NavigationContext { get; set; }
    }
}