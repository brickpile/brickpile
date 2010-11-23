using System.Web.Routing;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Web.Mvc {
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
        #region public static ContentRoute RegisterContentRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver)
        /// <summary>
        /// Responsible for setting up the content route for Stormbreaker
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="pathResolver"></param>
        /// <param name="virtualPathResolver"></param>
        /// <returns></returns>
        public static ContentRoute RegisterContentRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver)
        {
            var contentRoute = new ContentRoute(pathResolver, virtualPathResolver);
            routes.Add("Content", contentRoute);
            return contentRoute;
        }
        #endregion
    }
}