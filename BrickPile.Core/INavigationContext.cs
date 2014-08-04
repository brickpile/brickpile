using System.Collections.Generic;
using System.Web.Routing;

namespace BrickPile.Core
{
    /// <summary>
    ///     Defines the properties that are required for an <see cref="INavigationContext" />.
    /// </summary>
    public interface INavigationContext
    {
        /// <summary>
        ///     Gets the request context.
        /// </summary>
        /// <value>
        ///     The request context.
        /// </value>
        RequestContext RequestContext { get; }

        /// <summary>
        ///     Gets or sets the current page.
        /// </summary>
        /// <value>
        ///     The current page.
        /// </value>
        IPage CurrentPage { get; set; }

        /// <summary>
        ///     Gets or sets the current context.
        /// </summary>
        /// <value>
        ///     The current context.
        /// </value>
        IEnumerable<IPage> CurrentContext { get; set; }
    }
}