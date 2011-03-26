using System.Web.Routing;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// 
    /// </summary>
    internal static class RouteCollectionExtensions {
        /// <summary>
        /// Registers the page route.
        /// </summary>
        /// <param name="routes">The routes.</param>
        /// <param name="pathResolver">The path resolver.</param>
        /// <param name="virtualPathResolver">The virtual path resolver.</param>
        /// <returns></returns>
        internal static RouteCollection RegisterPageRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) {
            var pageRoute = new PageRoute(pathResolver, virtualPathResolver);
            routes.Add("PageRoute", pageRoute);
            return routes;
        }
    }
}