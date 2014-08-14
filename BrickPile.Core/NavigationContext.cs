using System;
using System.Collections.Generic;
using System.Linq;
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

            this.StartPage = this.RequestContext.RouteData.GetPages().SingleOrDefault(x => x.Parent == null);
            this.CurrentPage = this.RequestContext.RouteData.GetCurrentPage<IPage>();
            this.OpenPages = this.RequestContext.RouteData.GetPages();            
        }

        /// <summary>
        ///     Gets the request context.
        /// </summary>
        /// <value>
        ///     The request context.
        /// </value>
        public RequestContext RequestContext { get; set; }

        /// <summary>
        /// Gets or sets the start page.
        /// </summary>
        /// <value>
        /// The start page.
        /// </value>
        public IPage StartPage { get; set; }

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
        public IEnumerable<IPage> OpenPages { get; set; }
    }
}