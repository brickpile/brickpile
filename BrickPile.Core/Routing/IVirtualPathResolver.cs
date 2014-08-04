using System.Web.Routing;

namespace BrickPile.Core.Routing
{
    public interface IVirtualPathResolver {
        /// <summary>
        /// Resolves the virtual path.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        string ResolveVirtualPath(IPage page, RouteValueDictionary routeValueDictionary);
    }
}