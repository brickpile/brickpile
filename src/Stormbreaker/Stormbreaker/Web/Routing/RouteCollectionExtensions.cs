using System;
using System.Web.Routing;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class RouteCollectionExtensions
    {
        /* *******************************************************************
        *  Methods
        * *******************************************************************/
        public static RouteCollection RegisterDocumentRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver) {
            var contentRoute = new PageRoute(pathResolver, virtualPathResolver);
            routes.Add("DocumentRoute", contentRoute);
            return routes;
        }
    }
}