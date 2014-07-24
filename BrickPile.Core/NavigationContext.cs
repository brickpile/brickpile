using System;
using System.Web.Routing;
using BrickPile.Core.Routing;

namespace BrickPile.Core
{
    /// <summary>
    /// Represents the navigation context and contains the current page and it's child nodes, each ancestors and their child nodes.
    /// The navigation context is used to render the menu and sub menu
    /// </summary>
    public class NavigationContext
    {
        private readonly RequestContext _requestContext;

        public IPage[] CurrentContext {
            get {
                if (!_requestContext.RouteData.DataTokens.ContainsKey(RouteResolver.NavigationContextKey)) {
                    throw new Exception("The current request context does not contain the current navigation context.");
                }
                return _requestContext.RouteData.DataTokens[RouteResolver.NavigationContextKey] as IPage[];
            }
        }

        public NavigationContext(RequestContext requestContext) {
            _requestContext = requestContext;
        }
    }
}
