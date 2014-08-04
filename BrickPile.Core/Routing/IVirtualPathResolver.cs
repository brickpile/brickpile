using System.Web.Routing;

namespace BrickPile.Core.Routing
{
    /// <summary>
    ///     Defines the methods that are required for an <see cref="IVirtualPathResolver" />.
    /// </summary>
    internal interface IVirtualPathResolver
    {
        /// <summary>
        ///     Resolves the virtual path base on <see cref="IPage" />.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="routeValueDictionary">The route value dictionary.</param>
        /// <returns></returns>
        string ResolveVirtualPath(IPage page, RouteValueDictionary routeValueDictionary);
    }
}