using System.Collections.Generic;
using BrickPile.Core;

namespace BrickPile.UI.Web.ViewModels {
    /// <summary>
    /// Represents the view model
    /// </summary>
    public interface IViewModel<out T> {
        /// <summary>
        ///   <see cref="DefaultViewModel{T}.CurrentPage"/>
        /// </summary>
        T CurrentPage { get; }

        /// <summary>
        /// Gets or sets the hierarchy.
        /// </summary>
        /// <value>
        /// The hierarchy.
        /// </value>
        IEnumerable<IPage> NavigationContext { get; set; }
    }
}