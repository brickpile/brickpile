using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using BrickPile.Core.Extensions;

namespace BrickPile.Core.Routing
{
    internal class PageRoute : RouteBase
    {
        private readonly VirtualPathResolver _virtualPathResolver;
        private readonly IRouteResolver _routeResolver;
        public const string ControllerKey = "controller";

        /// <summary>
        /// Gets the model key.
        /// </summary>
        /// <value>
        /// The model key.
        /// </value>
        public static string CurrentPageKey
        {
            get { return "currentPage"; }
        }

        /// <summary>
        /// Gets the action key.
        /// </summary>
        /// <value>
        /// The action key.
        /// </value>
        public static string ActionKey
        {
            get { return "action"; }
        }

        /// <summary>
        /// Gets the default action.
        /// </summary>
        /// <value>
        /// The default action.
        /// </value>
        public static string DefaultAction
        {
            get { return "index"; }
        }

        /// <summary>
        /// When overridden in a derived class, returns route information about the request.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        /// An object that contains the values from the route definition if the route matches the current request, or null if the route does not match the request.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Abort and proceed to other routes in the route table if path contains api or ui
            var segments = httpContext.Request.Path.Split(new[] { '/' });
            if (segments.Any(segment => segment.Equals("api", StringComparison.OrdinalIgnoreCase) ||
                                        segment.Equals("ui", StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var routeData = _routeResolver.ResolveRoute(this, httpContext, httpContext.Request.Path);

            // Abort and proceed to other routes in the route table
            if (routeData == null)
            {
                return null;
            }

            var currentPage = routeData.Values["currentPage"] as IPage;

            // throw a proper 404 if the page is not published or if it's deleted
            if ((currentPage != null && (!currentPage.Metadata.IsPublished || currentPage.Metadata.IsDeleted) && !httpContext.User.Identity.IsAuthenticated))
            {
                throw new HttpException(404, "HTTP/1.1 404 Not Found");
            }

            return routeData;
        }

        /// <summary>
        /// When overridden in a derived class, checks whether the route matches the specified values, and if so, generates a URL and retrieves information about the route.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the requested route.</param>
        /// <param name="values">An object that contains the parameters for a route.</param>
        /// <returns>
        /// An object that contains the generated URL and information about the route, or null if the route does not match <paramref name="values" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            
            var model = values[CurrentPageKey] as IPage ?? requestContext.RouteData.Values[CurrentPageKey] as IPage;

            if (model == null)
            {
                return null;
            }

            var vpd = new VirtualPathData(this, _virtualPathResolver.ResolveVirtualPath(model, values));

            var queryParams = String.Empty;
            // add query string parameters
            foreach (var kvp in values)
            {
                if (kvp.Key.Equals(CurrentPageKey) || kvp.Key.Equals(ControllerKey) || kvp.Key.Equals(ActionKey))
                {
                    continue;
                }
                queryParams = queryParams.AddQueryParam(kvp.Key, kvp.Value.ToString());
            }
            vpd.VirtualPath += queryParams;
            return vpd;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRoute"/> class.
        /// </summary>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <param name="routeResolver"></param>
        public PageRoute(VirtualPathResolver virtualPathResolver, IRouteResolver routeResolver)
        {
            _virtualPathResolver = virtualPathResolver;
            _routeResolver = routeResolver;
        }

        
    }
}
