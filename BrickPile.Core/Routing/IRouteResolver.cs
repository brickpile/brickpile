using System.Web;
using System.Web.Routing;

namespace BrickPile.Core.Routing {
    public interface IRouteResolver {
        /// <summary>
        /// Resolves the route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        RouteData ResolveRoute(RouteBase route, HttpContextBase httpContext, string virtualPath);
    }
}