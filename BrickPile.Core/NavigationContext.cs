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
        public RequestContext RequestContext { get; private set; }

        public IPage CurrentPage { get; set; }

        public IEnumerable<IPage> CurrentContext { get; set; }

        public NavigationContext() {}

        public NavigationContext(RequestContext requestContext) {
            this.RequestContext = requestContext;

            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            this.CurrentContext = this.RequestContext.RouteData.GetCurrentContext();
            this.CurrentPage = this.RequestContext.RouteData.GetCurrentPage<IPage>();
        }
    }
}