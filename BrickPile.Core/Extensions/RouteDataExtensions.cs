using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using BrickPile.Core.Routing;

namespace BrickPile.Core.Extensions
{
    /// <summary>
    ///     Provides BrickPile <see cref="RouteData" /> helper methods.
    /// </summary>
    public static class RouteDataExtensions
    {
        /// <summary>
        ///     Gets the current page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T GetCurrentPage<T>(this RouteData data)
        {
            return (T) data.Values[PageRoute.CurrentPageKey];
        }

        /// <summary>
        ///     Applies the structure information.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="structureInfo">The structure information.</param>
        internal static void ApplyStructureInfo(this RouteData routeData, StructureInfo structureInfo)
        {
            routeData.DataTokens["brickpile:structureInfo"] = structureInfo;
        }

        /// <summary>
        ///     Applies the current context.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="pages">The pages.</param>
        internal static void ApplyCurrentContext(this RouteData routeData, IEnumerable<IPage> pages)
        {
            routeData.DataTokens["brickpile:currentcontext"] = pages;
        }

        /// <summary>
        ///     Gets the current context.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        public static IEnumerable<IPage> GetCurrentContext(this RouteData routeData)
        {
            return (IEnumerable<IPage>) routeData.DataTokens["brickpile:currentcontext"];
        }

        /// <summary>
        ///     Gets the navigation context.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        public static NavigationContext GetNavigationContext(this RouteData routeData)
        {
            return new NavigationContext(HttpContext.Current.Request.RequestContext);
        }

        /// <summary>
        ///     Gets the structure information.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        public static StructureInfo GetStructureInfo(this RouteData routeData)
        {
            return (StructureInfo) routeData.DataTokens["brickpile:structureInfo"];
        }
    }
}