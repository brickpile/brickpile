using System;
using System.Collections.Generic;
using System.Web.Routing;
using BrickPile.Core.Extensions;

namespace BrickPile.Core
{
    /// <summary>
    ///     Represents the navigation context and contains the current page and it's child nodes, each ancestors and their
    ///     child nodes.
    ///     The navigation context is used to render the menu and sub menu
    /// </summary>
    public class NavigationContext : INavigationContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NavigationContext" /> class.
        /// </summary>
        public NavigationContext() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="NavigationContext" /> class.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <exception cref="System.ArgumentNullException">requestContext</exception>
        public NavigationContext(RequestContext requestContext)
        {
            this.RequestContext = requestContext;

            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            this.CurrentContext = this.RequestContext.RouteData.GetCurrentContext();
            this.CurrentPage = this.RequestContext.RouteData.GetCurrentPage<IPage>();
        }

        /// <summary>
        ///     Gets the request context.
        /// </summary>
        /// <value>
        ///     The request context.
        /// </value>
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        ///     Gets or sets the current page.
        /// </summary>
        /// <value>
        ///     The current page.
        /// </value>
        public IPage CurrentPage { get; set; }

        /// <summary>
        ///     Gets or sets the current context.
        /// </summary>
        /// <value>
        ///     The current context.
        /// </value>
        public IEnumerable<IPage> CurrentContext { get; set; }
    }
}