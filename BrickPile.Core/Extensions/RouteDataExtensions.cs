using System.Collections.Generic;
using System.Web.Routing;
using BrickPile.Core.Routing;
using BrickPile.Core.Routing.Trie;

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
            return (T) data.Values[DefaultRoute.CurrentPageKey];
        }

        /// <summary>
        /// Applies the structure information.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="trie">The trie.</param>
        internal static void ApplyTrie(this RouteData routeData, Trie trie)
        {
            routeData.DataTokens["brickpile:trie"] = trie;
        }

        /// <summary>
        /// Applies the current context.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="context">The context.</param>
        internal static void ApplyCurrentContext(this RouteData routeData, BrickPileContext context)
        {
            routeData.DataTokens["brickpile:context"] = context;
        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        internal static IPage[] GetPages(this RouteData routeData)
        {
            return (IPage[])routeData.DataTokens["brickpile:pages"];
        }

        /// <summary>
        /// Applies the pages.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="pages">The pages.</param>
        internal static void ApplyPages(this RouteData routeData, IEnumerable<IPage> pages)
        {
            routeData.DataTokens["brickpile:pages"] = pages;
        }

        /// <summary>
        ///     Gets the structure information.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns></returns>
        public static Trie GetTrie(this RouteData routeData)
        {
            return (Trie) routeData.DataTokens["brickpile:trie"];
        }
    }
}