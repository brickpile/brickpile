using System.Web.Routing;
using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    public interface IVirtualPathResolver {
        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        string ResolveVirtualPath(IPageModel pageModel, RouteValueDictionary routeValueDictionary);
    }
}